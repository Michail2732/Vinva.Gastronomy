using System.Security.Claims;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.GetCurrentUser
{
    public readonly record struct GetCurrentUserQueryResponse 
    {
        public string Login { get; init; }
        public string Email { get; init; }
        public UserState State { get; init; }
    }
}
