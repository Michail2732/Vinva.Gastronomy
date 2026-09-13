using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vinva.Gastronomy.Common.Entities;
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
        private readonly IPasswordService _passwordHashService;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly ISessionService _sessionService;
        private readonly TimeProvider _timeProvider;

        public LoginHandler(IPasswordService passwordHashService, ITokenService tokenService,
            TimeProvider timeProvider, IdentityDbContext dbContext, 
            IHashService hashService, ISessionService sessionService)
        {
            _passwordHashService = passwordHashService ?? throw new ArgumentNullException(nameof(passwordHashService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        public async Task<LoginResponce> Handle(LoginRequest request, CancellationToken cancellationToken)
        {            
            Expression<Func<User, bool>> searchSpec = a => a.Login == request.Login;

            var user = await _dbContext.Users.Include(a => a.Session)
                .FirstOrDefaultAsync(searchSpec, cancellationToken)
                ?? throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

            var crntTime = _timeProvider.GetUtcNow();            

            if (!_passwordHashService.VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

            if (user.State == UserState.Blocked)
                throw new UnauthorizedException(IdentityApplicationErrors.UserIsBlocked.Description);

            user.Session = new UserSession(user.Id, request.DeviceId)
            {
                ExpiresAt = _timeProvider.GetUtcNow().AddDays(1)
            };

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);
            user.Session.RefreshTokenHash = _hashService.Hash(refreshToken);
                        
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            _sessionService.AddOrUpdateSession(user.Session);

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
    }
}
