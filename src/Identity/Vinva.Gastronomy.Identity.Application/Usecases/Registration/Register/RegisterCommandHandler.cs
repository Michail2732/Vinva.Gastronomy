using System.Net;
using System.Net.Mail;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Registration.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IRegistrationService _registrationService;
        private readonly RegisterSmtpConfig _registerConfig;        

        public RegisterCommandHandler(IdentityDbContext identityUnitOfWork,
            IRegistrationService registrationService,
            IOptions<RegisterSmtpConfig> emailConfig,
            IPasswordHashService passwordHashService)
        {
            _dbContext = identityUnitOfWork ?? throw new ArgumentNullException(nameof(identityUnitOfWork));
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _registerConfig = emailConfig?.Value ?? throw new ArgumentNullException(nameof(emailConfig));
            _passwordHashService = passwordHashService ?? throw new ArgumentNullException(nameof(passwordHashService));
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var validator = new RegisterCommandValidator();
            var validResult = await validator.ValidateAsync(request, cancellationToken);

            if (validResult.IsValid)
                validResult.HandleValidationErrors<Result>();
            
            var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Email == request.Email, cancellationToken);
            if (user != null)
                throw new BadRequestException(IdentityApplicationErrors.UserWithSameEmailExists);

            var passwordHash = _passwordHashService.HashPassword(request.Password);
            var token = await _registrationService.GenerateTokenAsync(request.Email, request.Login, passwordHash, cancellationToken);

            var smtpClient = new SmtpClient(_registerConfig.SmtpHost, _registerConfig.SmtpPort)
            {
                Credentials = new NetworkCredential(
                    _registerConfig.SmtpCredentialAddress,
                    _registerConfig.SmtpCredentialPassword),
                EnableSsl = true
            };
            
            try
            {
                var confirmLink = await _registrationService.GenerateRegisterConfirmLinkTokenAsync(token.Id);
                var message = _registerConfig.GetMailBody(request.Login, confirmLink); 
                smtpClient.Send(_registerConfig.From, request.Email, _registerConfig.MailSubject, message);
            }
            catch (Exception ex)
            {
                throw;
            }                                    
            smtpClient.Dispose();            
        }
    }
}
