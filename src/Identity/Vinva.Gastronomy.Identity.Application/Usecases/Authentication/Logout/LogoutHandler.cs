using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence;


namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutRequest>
    {
        private readonly IdentityDbContext _dbContext;        
        private readonly TimeProvider _timeProvider;
        private readonly IHashService _hashService;
        private readonly ISessionService _sessionService;
        private readonly ITokenService _tokenService;

        public LogoutHandler(IdentityDbContext dbContext, TimeProvider timeProvider,
         IHashService hashService, ISessionService sessionService, ITokenService tokenService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task Handle(LogoutRequest request, CancellationToken ct)
        {
            var claims = await _tokenService.ValidateTokenAsync(request.RefreshToken, ct);
            var userIdValue = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdValue, out var userId))
                return;

            var user =  await _dbContext.Users
                        .Include(a => a.Session)
                        .Include(a => a.Registration)
                        .FirstOrDefaultAsync(a => a.Id == userId, ct);
            
            
            if (user == null || user.Session is null ||
                !_hashService.Verify(request.RefreshToken, user.Session.RefreshTokenHash!))
                return;


            user.Session!.Revoke(_timeProvider.GetUtcNow());

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(ct);

            _sessionService.RemoveSession(user.Session.Id);
                        
        }
    }
}
