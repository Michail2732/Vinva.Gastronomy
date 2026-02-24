using MediatR;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.GetCurrentUser
{
    internal sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserQueryResponse>
    {
        public Task<GetCurrentUserQueryResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            // Implement your logic here
            throw new NotImplementedException();
        }
    }
}
