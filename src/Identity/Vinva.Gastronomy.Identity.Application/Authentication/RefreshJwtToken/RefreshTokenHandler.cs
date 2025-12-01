using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
using Vinva.Gastronomy.Identity.Application.Authentication.Logout;
using Vinva.Gastronomy.Identity.Application.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence.Repositories;
using Vinva.Gastronomy.Identity.Persistence.Specifications;

namespace Vinva.Gastronomy.Identity.Application.Authentication.RefreshJwtToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, Result<RefreshTokenResponce>>
    {
        private readonly IUserTokensRepository _userTokensRepository;
        private readonly ITokenService _tokenService;
        private readonly IUsersRepository _usersRepository;

        public RefreshTokenHandler(IUserTokensRepository userTokensRepository, 
            ITokenService tokenService, IUsersRepository usersRepository)
        {
            _userTokensRepository = userTokensRepository ?? throw new ArgumentNullException(nameof(userTokensRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _usersRepository = usersRepository;
        }

        public async Task<Result<RefreshTokenResponce>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var validator = new RefreshTokenRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return validationResult.HandleValidationErrors<RefreshTokenResponce>();

            var userTokensSpec = new ByRefreshTokenSpec(request.RefreshToken);
            var userTokens = await _userTokensRepository.FirstOrDefaultAsync(userTokensSpec, cancellationToken);

            if (userTokens == null)
                throw new UnauthorizedException(IdentityApplicationErrors.RefreshTokenInvalid.Description);

            var user = await _usersRepository.GetByIdAsync(userTokens.UserId, cancellationToken);

            if (user == null)
                throw new InvalidOperationException($"Could not found user with id '{userTokens.UserId}'");

            if (user.State == UserState.Blocked)
                return Result.Failure<RefreshTokenResponce>(IdentityApplicationErrors.RefreshTokenInvalid);

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user, cancellationToken);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user, cancellationToken);
            var expiresAt = await _tokenService.GetTokenExpirationAsync(accessToken, cancellationToken);

            userTokens.SetNewToken(accessToken, refreshToken, expiresAt);
            await _userTokensRepository.UpdateAsync(userTokens, cancellationToken);

            return new RefreshTokenResponce
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                Login = user.Login,
                Role = user.Role
            };
        }
    }
}
