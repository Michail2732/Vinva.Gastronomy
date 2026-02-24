using MediatR;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.GetCurrentUser
{
    // Include properties to be used as input for the query
    public readonly record struct GetCurrentUserQuery : IRequest<GetCurrentUserQueryResponse>
    {
        public string AccessToken { get; init; }
    }
}
