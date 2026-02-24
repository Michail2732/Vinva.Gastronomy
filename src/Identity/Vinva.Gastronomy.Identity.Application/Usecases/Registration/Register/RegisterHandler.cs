using System.Net;
using System.Net.Mail;
using MediatR;
using Microsoft.Extensions.Options;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Application.RegUsecases.Register;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence.Repositories;
using Vinva.Gastronomy.Identity.Persistence.Specifications;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Registration.Register
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, Result>
    {
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IRegistrationService _registrationService;
        private readonly RegisterSmtpConfig _registerConfig;

        public RegisterHandler(IIdentityUnitOfWork identityUnitOfWork,
            IRegistrationService registrationService,
            IOptions<RegisterSmtpConfig> emailConfig,
            IPasswordHashService passwordHashService)
        {
            _identityUnitOfWork = identityUnitOfWork ?? throw new ArgumentNullException(nameof(identityUnitOfWork));
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _registerConfig = emailConfig?.Value ?? throw new ArgumentNullException(nameof(emailConfig));
            _passwordHashService = passwordHashService ?? throw new ArgumentNullException(nameof(passwordHashService));
        }

        public async Task<Result> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var validator = new RegisterRequestValidator();
            var validResult = await validator.ValidateAsync(request, cancellationToken);

            if (validResult.IsValid)
                validResult.HandleValidationErrors<Result>();

            var byEmailSpec = new ByEmailSpec(request.Email);
            var user = await _identityUnitOfWork.Users.FirstOrDefaultAsync(byEmailSpec, cancellationToken);
            if (user != null)
                return Result.Failure(IdentityApplicationErrors.UserWithSameEmailExists);

            var passwordHash = _passwordHashService.HashPassword(request.Password);
            var token = await _registrationService.GenerateTokenAsync(request.Email, request.Login, passwordHash, cancellationToken);

            var smtpClient = new SmtpClient(_registerConfig.SmtpHost,
                _registerConfig.SmtpPort);
            var credentials = new NetworkCredential(_registerConfig.SmtpCredentialAddress, 
                _registerConfig.SmtpCredentialPassword);

            smtpClient.Credentials = credentials;
            var message = new MailMessage()
            {
                Sender = new MailAddress(_registerConfig.SmtpCredentialAddress),
                Subject = _registerConfig.MailSubject,
                Body = _registerConfig.GetMailBody(request.Login, token.Id)
            };
            message.To.Add(request.Email);
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                throw new ScenarioException(IdentityApplicationErrors.CantSendRegistrationMessage.Description, ex);
            }    
            smtpClient.Dispose();
            return Result.Success();
        }
    }
}
