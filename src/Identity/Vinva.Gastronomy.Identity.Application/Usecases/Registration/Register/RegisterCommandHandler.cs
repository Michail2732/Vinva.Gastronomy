using System.Net;
using System.Net.Mail;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            var query = from tokenQ in _dbContext.RegistrationTokens
                        where tokenQ.Email == request.Email
                        join userQ in _dbContext.Users on tokenQ.Email equals userQ.Email into userJoin
                        from userQ in userJoin.DefaultIfEmpty()
                        select new { Token = tokenQ, User = userQ };

            var result = await query.FirstOrDefaultAsync(cancellationToken);
            var token = result?.Token;
            var user = result?.User;

            if (user != null)
                throw new BadRequestException(IdentityApplicationErrors.UserWithSameEmailAlreadyExists);
            if (token != null && !token.IsExpires(_timeProvider) && token.IsConfirmLetterSent)
                throw new BadRequestException(IdentityApplicationErrors.RegistrationTokenNotExpires);
            

            if (!_passwordHashService.IsValidPassword(request.Password))
                throw new BadRequestException($"Пароль не соответствует требованиям: {_passwordHashService.GetPasswordRequirements()}");

            var passwordHash = _passwordHashService.HashPassword(request.Password);

            if (token != null)
            {                
                await _registrationService.UpdateTokenAsync(token, passwordHash);
                _dbContext.RegistrationTokens.Update(token);

            }
            else
            {
                token = await _registrationService.GenerateTokenAsync(request.Email, request.Login, passwordHash, cancellationToken);
                await _dbContext.RegistrationTokens.AddAsync(token);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

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
                token.LetterSent(_timeProvider);
                _dbContext.RegistrationTokens.Update(token);
                await _dbContext.SaveChangesAsync(cancellationToken);
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
                Details = "Регистрация запущена. Для подтверждения регистрации следуйте инструкциям отправленным в письме."
            };
        }
    }
}
