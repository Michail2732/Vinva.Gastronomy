using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.GetCurrentUser
{
    public sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserQueryResponse>
    {
        private readonly ITokenService _tokenService;

        public GetCurrentUserQueryHandler(ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<GetCurrentUserQueryResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var claims = await _tokenService.ValidateTokenAsync(request.AccessToken, cancellationToken);
            if (claims == null)
                throw new BadRequestException(IdentityApplicationErrors.ValidationFailed);

            var parser = new UserClaimsParcer();            

            return new GetCurrentUserQueryResponse
            {
                Id = parser.ParseId(claims),
                Login = parser.ParseLogin(claims),
                Email = parser.ParseEmail(claims),
                Roles = parser.ParseRoles(claims),
                State = parser.ParseState(claims)
            };

        }
    }
}
