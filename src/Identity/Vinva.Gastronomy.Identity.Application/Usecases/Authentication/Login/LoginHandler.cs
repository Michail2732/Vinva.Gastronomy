using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponce>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IPasswordHashService _passwordHashService;
        private readonly ITokenService _tokenService;
        private readonly TimeProvider _timeProvider;

        public LoginHandler(IPasswordHashService passwordHashService, ITokenService tokenService,
            TimeProvider timeProvider, IdentityDbContext dbContext)
        {
            _passwordHashService = passwordHashService ?? throw new ArgumentNullException(nameof(passwordHashService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));            
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<LoginResponce> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new LoginRequestValidator();

                var validResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validResult.IsValid)
                {
                    var errors = validResult.HandleValidationErrors<LoginResponce>().Error;
                    throw new BadRequestException($"{errors.Code}.{errors.Description}");
                }                    

                var passHash = _passwordHashService.HashPassword(request.Password);

                Expression<Func<User, bool>> searchSpec = a => a.Login == request.Login;

                var user = await _dbContext.Users.FirstOrDefaultAsync(searchSpec, cancellationToken)
                    ?? throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

                if (!_passwordHashService.VerifyPassword(request.Password, passHash))
                    throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

                var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);
                var expiresAt = await _tokenService.GetTokenExpirationAsync(accessToken);

                user.LastLoginAt = _timeProvider.GetUtcNow();

                using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
                {
                    _dbContext.Users.Update(user);

                    await UpdateUserTokens(user, accessToken, refreshToken, expiresAt, cancellationToken);

                    await transaction.CommitAsync(cancellationToken);

                    return new LoginResponce
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresAt = expiresAt,
                        Login = user.Login,
                        Role = user.Role
                    };
                }                
            }
            catch (Exception)
            {
                if (_dbContext.Database.CurrentTransaction != null)
                    await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
                throw;
            }            
        }

        private async Task UpdateUserTokens(User user, string accessToken, string refreshToken,
            DateTimeOffset expiresAt, CancellationToken ct = default)
        {
            var userToken = await _dbContext.UserTokens.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (userToken == null)
            {
                userToken = new UserTokens(user.Id, accessToken, refreshToken, expiresAt);
                await _dbContext.UserTokens.AddAsync(userToken, ct);
            }
            else
            {
                userToken.SetNewToken(accessToken, refreshToken, expiresAt);
                _dbContext.UserTokens.Update(userToken);
            }            
        }
    }
}
