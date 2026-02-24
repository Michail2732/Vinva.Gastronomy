using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Persistence.Repositories;
using Vinva.Gastronomy.Identity.Persistence.Specifications;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutRequest, Result>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUserTokensRepository _userTokensRepository;        
        private readonly TimeProvider _timeProvider;

        public LogoutHandler(IUsersRepository usersRepository, 
            IUserTokensRepository userTokensRepository, TimeProvider timeProvider)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _userTokensRepository = userTokensRepository ?? throw new ArgumentNullException(nameof(userTokensRepository));            
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public async Task<Result> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            var searchSpec = new ByLoginAndPasswordHashSpec(request.Login);

            var user = await _usersRepository.FirstOrDefaultAsync(searchSpec, cancellationToken);
            if (user == null)
                throw new UnauthorizedException(IdentityApplicationErrors.InvalidCredentials.Description);

            user.LastLogoutAt = _timeProvider.GetUtcNow();

            await _usersRepository.UpdateAsync(user);            

            var userToken = await _userTokensRepository.GetByIdAsync(user.Id, cancellationToken);

            if (userToken == null)
                return Result.Success();

            userToken.ResetToken();
            await _userTokensRepository.UpdateAsync(userToken, cancellationToken);            

            return Result.Success();

        }
    }
}
