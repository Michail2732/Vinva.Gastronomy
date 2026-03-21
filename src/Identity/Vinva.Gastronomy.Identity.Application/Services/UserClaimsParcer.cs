using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Domain.Services
{
    public class UserClaimsParcer
    {
        public const string StateType = "State";

        
        public Guid ParseId(ClaimsPrincipal principal)
        {
            var idStr = principal.Claims.First(a => a.Type == ClaimTypes.NameIdentifier).Value;
            return Guid.Parse(idStr);
        }

        public UserState ParseState(ClaimsPrincipal principal)
        {
            var stateStr = principal.Claims.First(a => a.Type == StateType).Value;
            return Enum.Parse<UserState>(stateStr);
        }

        public UserRole[] ParseRoles(ClaimsPrincipal principal)
        {
            List<UserRole> roles = new List<UserRole>();
            foreach (var roleClaim in principal.Claims.Where(a => a.Type == ClaimTypes.Role))
            {
                roles.Add(Enum.Parse<UserRole>(roleClaim.Value));
            }
            return roles.ToArray();
        }

        public string ParseLogin(ClaimsPrincipal principal)
        {
            return principal.Claims.First(a => a.Type == ClaimTypes.Name).Value;
        }

        public string ParseEmail(ClaimsPrincipal principal)
        {
            return principal.Claims.First(a => a.Type == ClaimTypes.Email).Value;
        }
                
        public UserInfo Parse(ClaimsPrincipal principal)
        {
            var idStr = principal.Claims.First(a => a.Type == JwtRegisteredClaimNames.Sub).Value;
            var stateStr = principal.Claims.First(a => a.Type == StateType).Value;

            List<UserRole> roles = new List<UserRole>();
            foreach (var roleClaim in principal.Claims.Where(a => a.Type == ClaimTypes.Role))
            {
                roles.Add(Enum.Parse<UserRole>(roleClaim.Value));
            }

            return new UserInfo
            {
                Id = Guid.Parse(idStr),
                Email = principal.Claims.First(a => a.Type == ClaimTypes.Email).Value,
                Login = principal.Claims.First(a => a.Type == ClaimTypes.Name).Value,
                Roles = roles.ToArray(),
                State = Enum.Parse<UserState>(stateStr)
            };
        }

        public List<Claim> Parse(UserInfo user)
        {
            var claims =  new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Login),
                new(StateType, user.State.ToString()),
                new(ClaimTypes.Email, user.Email),                
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new (ClaimTypes.Role, role.ToString()));
            }
            return claims;
        }

        public List<Claim> Parce(User user)
        {
            var userInfo = new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                Login = user.Login,
                State = user.State,
                Roles = user.Roles.ToArray(),
            };
            return Parse(userInfo);
        }
    }
}
