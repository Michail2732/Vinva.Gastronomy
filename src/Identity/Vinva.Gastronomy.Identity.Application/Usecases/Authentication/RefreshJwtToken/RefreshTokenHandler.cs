using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.RefreshJwtToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponce>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly ITokenService _tokenService;        
        private readonly IHashService _hashService;
        private readonly ISessionService _sessionService;

        public RefreshTokenHandler(IdentityDbContext dbContext,
            ITokenService tokenService,
            IHashService hashService,
            ISessionService sessionService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        }

        public async Task<RefreshTokenResponce> Handle(RefreshTokenRequest request, CancellationToken ct)
        {

            var claims = await _tokenService.ValidateTokenAsync(request.RefreshToken, ct);
            var userIdValue = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdValue, out var userId))
                throw new UnauthorizedException(IdentityApplicationErrors.RefreshTokenInvalid);

            var user =  await _dbContext.Users
                        .Include(a => a.Session)
                        .Include(a => a.Registration)
                        .FirstOrDefaultAsync(a => a.Id == userId, ct);
            
            
            if (user == null)
                throw new UnauthorizedException(IdentityApplicationErrors.UserCouldNotFound);

            if (user.State == UserState.Blocked)
                throw new ForbiddenException(IdentityApplicationErrors.RefreshTokenInvalid);

            if (user.Session is null || !user.Session.IsActive(DateTimeOffset.UtcNow) ||
                !_hashService.Verify(request.RefreshToken, user.Session.RefreshTokenHash!))
                throw new UnauthorizedException(IdentityApplicationErrors.RefreshTokenInvalid);

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user, ct);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user, ct);  

            
            var refreshTokenHash = _hashService.Hash(refreshToken);

            user.Session!.RefreshTokenHash = refreshTokenHash;

            _dbContext.Sessions.Update(user.Session);
            await _dbContext.SaveChangesAsync();

            _sessionService.AddOrUpdateSession(user.Session);

            return new RefreshTokenResponce
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,                
                Login = user.Login,
                Roles = user.Roles,
                Email = user.Email,
                Id = user.Id,
                State = user.State
            };
        }
    }
}
