using System.Security.Claims;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.GetCurrentUser
{
    public record GetCurrentUserQueryResponse: UserInfoDto 
    {        
    }
}
