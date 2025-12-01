using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Identity.Application.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence.Repositories;
using Vinva.Gastronomy.Identity.Persistence.Specifications;

namespace Vinva.Gastronomy.Identity.Application.Authentication.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, Result<LoginResponce>>
    {
        private readonly IIdentityUnitOfWork _identityUnitOfWork;
        private readonly IPasswordHashService _passwordHashService;
        private readonly ITokenService _tokenService;
        private readonly TimeProvider _timeProvider;

        public LoginHandler(IPasswordHashService passwordHashService, ITokenService tokenService,
            TimeProvider timeProvider, IIdentityUnitOfWork identityUnitOfWork)
        {
            _passwordHashService = passwordHashService ?? throw new ArgumentNullException(nameof(passwordHashService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));            
            _identityUnitOfWork = identityUnitOfWork ?? throw new ArgumentNullException(nameof(identityUnitOfWork));
        }

        public async Task<Result<LoginResponce>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new LoginRequestValidator();

                var validResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validResult.IsValid)
                    return validResult.HandleValidationErrors<LoginResponce>();

                var passHash = _passwordHashService.HashPassword(request.Password);
                var searchSpec = new ByLoginAndPasswordHashSpec(request.Login);

                var user = await _identityUnitOfWork.Users.FirstOrDefaultAsync(searchSpec, cancellationToken)
                    ?? throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

                if (!_passwordHashService.VerifyPassword(request.Password, passHash))
                    throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

                var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);
                var expiresAt = await _tokenService.GetTokenExpirationAsync(accessToken);

                user.LastLoginAt = _timeProvider.GetUtcNow();

                await _identityUnitOfWork.BeginTransactionAsync(cancellationToken);
                await _identityUnitOfWork.Users.UpdateAsync(user);

                await UpdateUserTokens(user, accessToken, refreshToken, expiresAt, cancellationToken);

                await _identityUnitOfWork.SaveAndCommitAsync(cancellationToken);

                return new LoginResponce
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt,
                    Login = user.Login,
                    Role = user.Role
                };                
            }
            catch (Exception)
            {
                if (_identityUnitOfWork.IsTransactionOpen())
                    await _identityUnitOfWork.RollbackAsync(cancellationToken);
                throw;
            }            
        }

        private async Task UpdateUserTokens(User user, string accessToken, string refreshToken,
            DateTimeOffset expiresAt, CancellationToken ct = default)
        {
            var userToken = await _identityUnitOfWork.UserTokens.GetByIdAsync(user.Id);
            if (userToken == null)
            {
                userToken = new UserTokens(user.Id, accessToken, refreshToken, expiresAt);
                await _identityUnitOfWork.UserTokens.AddAsync(userToken, ct);
            }
            else
            {
                userToken.SetNewToken(accessToken, refreshToken, expiresAt);
                await _identityUnitOfWork.UserTokens.UpdateAsync(userToken, ct);
            }            
        }
    }
}
