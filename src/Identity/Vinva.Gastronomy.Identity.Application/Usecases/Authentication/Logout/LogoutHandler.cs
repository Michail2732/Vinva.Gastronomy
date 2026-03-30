using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Persistence;


namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutRequest>
    {
        private readonly IdentityDbContext _dbContext;        
        private readonly TimeProvider _timeProvider;

        public LogoutHandler(IdentityDbContext dbContext, TimeProvider timeProvider)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public async Task Handle(LogoutRequest request, CancellationToken ct)
        {
            var query = from tokenQ in _dbContext.UserTokens
                        where tokenQ.AccessToken == request.AccessToken
                        join userQ in _dbContext.Users on tokenQ.UserId equals userQ.Id into userJoin
                        from userQ in userJoin.DefaultIfEmpty()
                        select new { Tokens = tokenQ, User = userQ };

            var result = await query.FirstOrDefaultAsync(ct);
            var tokens = result?.Tokens;
            var user = result?.User;
            
            if (tokens == null)
                return;            
            if (user == null)
                throw new Exception($"Token exists, but user not found: userId '{tokens.UserId}'");

            user.LastLogoutAt = _timeProvider.GetUtcNow();
            _dbContext.Users.Update(user);
            tokens.ResetToken();
            _dbContext.UserTokens.Update(tokens);
            await _dbContext.SaveChangesAsync(ct);            
        }
    }
}
