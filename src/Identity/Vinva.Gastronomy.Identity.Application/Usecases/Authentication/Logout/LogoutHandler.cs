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

        public async Task Handle(LogoutRequest request, CancellationToken cancellationToken)
        {            
            var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Login == request.Login, cancellationToken);
            if (user == null)
                throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

            user.LastLogoutAt = _timeProvider.GetUtcNow();

            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
                {
                    _dbContext.Users.Update(user);

                    var userToken = await _dbContext.UserTokens.FirstOrDefaultAsync(a => a.UserId == user.Id, cancellationToken);

                    if (userToken == null)
                        return;

                    userToken.ResetToken();
                    _dbContext.UserTokens.Update(userToken);
                    await transaction.CommitAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                if (_dbContext.Database.CurrentTransaction != null)
                    await _dbContext.Database.CurrentTransaction.RollbackAsync();
                throw;
            }            
        }
    }
}
