using System.Security.Claims;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.GetCurrentUser
{
    public readonly record struct GetCurrentUserQueryResponse 
    {
        public required Guid Id { get; init; }
        public required string Login { get; init; }
        public required string Email { get; init; }
        public required UserState State { get; init; }
        public required UserRole[] Roles { get; init; }
    }
}
