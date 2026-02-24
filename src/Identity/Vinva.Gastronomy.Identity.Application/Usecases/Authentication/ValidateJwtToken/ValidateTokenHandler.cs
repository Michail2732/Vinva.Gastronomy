using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Identity.Application.Authentication.RegisterConfirm;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Services;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.ValidateJwtToken
{
    public class ValidateTokenHandler : IRequestHandler<ValidateTokenRequest, Result>
    {
        private readonly ITokenService _tokenService;

        public ValidateTokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<Result> Handle(ValidateTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _tokenService.ValidateTokenAsync(request.AccessToken, cancellationToken);
            if (result is null)
                throw new UnauthorizedAccessException(IdentityApplicationErrors.TokenInvalid.Description);
            return Result.Success();
        }
    }
}
