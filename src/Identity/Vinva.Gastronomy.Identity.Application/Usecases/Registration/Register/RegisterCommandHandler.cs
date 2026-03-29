using System.Net;
using System.Net.Mail;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Registration.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponce>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IRegistrationService _registrationService;
        private readonly RegisterSmtpConfig _registerConfig;
        private readonly TimeProvider _timeProvider;

        public RegisterCommandHandler(IdentityDbContext identityUnitOfWork,
            IRegistrationService registrationService,
            IOptions<RegisterSmtpConfig> emailConfig,
            IPasswordHashService passwordHashService,
            TimeProvider timeProvider)
        {
            _dbContext = identityUnitOfWork ?? throw new ArgumentNullException(nameof(identityUnitOfWork));
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _registerConfig = emailConfig?.Value ?? throw new ArgumentNullException(nameof(emailConfig));
            _passwordHashService = passwordHashService ?? throw new ArgumentNullException(nameof(passwordHashService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public async Task<RegisterCommandResponce> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var token = await _dbContext.RegistrationTokens.FirstOrDefaultAsync(a => a.Email == request.Email, cancellationToken);
            bool isTokenExists = token != null;
            if (token != null && !token.IsExpires(_timeProvider))
                throw new BadRequestException(IdentityApplicationErrors.RegistrationTokenNotExpires);
            

            if (!_passwordHashService.IsValidPassword(request.Password))
                throw new BadRequestException($"Пароль не соответствует требованиям: {_passwordHashService.GetPasswordRequirements()}");

            var passwordHash = _passwordHashService.HashPassword(request.Password);

            if (token != null)
            {
                await _registrationService.UpdateTokenAsync(token, passwordHash);
            }
            else
            {
                token = await _registrationService.GenerateTokenAsync(request.Email, request.Login, passwordHash, cancellationToken);
            }            

            var smtpClient = new SmtpClient(_registerConfig.SmtpHost, _registerConfig.SmtpPort)
            {
                Credentials = new NetworkCredential(
                    _registerConfig.SmtpCredentialAddress,
                    _registerConfig.SmtpCredentialPassword),
                EnableSsl = true
            };
            
            try
            {
                _dbContext.RegistrationTokens.AddAsync();
                var confirmLink = await _registrationService.GenerateRegisterConfirmLinkTokenAsync(token.Id);
                var message = _registerConfig.GetMailBody(request.Login, confirmLink); 
                smtpClient.Send(_registerConfig.From, request.Email, _registerConfig.MailSubject, message);
            }
            catch (Exception)
            {
                throw;
            }       
            finally
            {
                smtpClient.Dispose();
            }
            return new RegisterCommandResponce
            {
                Details = "Ваша заявка на подтверждение регистрации получена. Для подтверждения регистрации следуйте инструкциям, отправленным в письме."
            };
        }
    }
}
