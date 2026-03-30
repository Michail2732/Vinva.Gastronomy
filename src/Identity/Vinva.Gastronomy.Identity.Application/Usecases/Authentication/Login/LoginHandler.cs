using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Exceptions;
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
            var passHash = _passwordHashService.HashPassword(request.Password);

            Expression<Func<User, bool>> searchSpec = a => a.Login == request.Login;

            var user = await _dbContext.Users.Include(a => a.Tokens)
                .FirstOrDefaultAsync(searchSpec, cancellationToken)
                ?? throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

            if (!_passwordHashService.VerifyPassword(request.Password, passHash))
                throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);            

            user.LastLoginAt = _timeProvider.GetUtcNow();

            UpdateUserTokens(user, accessToken, refreshToken);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new LoginResponce
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,                
                Login = user.Login,
                Roles = user.Roles,
                Id = user.Id,
                Email = user.Email,
                State = user.State
            };            
        }

        private void UpdateUserTokens(User user, string accessToken, string refreshToken)
        {                        
            if (user.Tokens == null)            
                user.Tokens = new UserTokens(user.Id, accessToken, refreshToken);            
            else            
                user.Tokens.SetNewToken(accessToken, refreshToken);                     
        }
    }
}
