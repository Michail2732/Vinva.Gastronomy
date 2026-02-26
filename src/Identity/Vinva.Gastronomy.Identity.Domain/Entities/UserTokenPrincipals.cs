using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Domain.Entities
{
    public class UserTokenPrincipals
    {
        public ClaimsPrincipal Principal { get; }

        public UserTokenPrincipals(ClaimsPrincipal principal)
        {
            Principal = principal ?? throw new ArgumentNullException(nameof(principal));
        }

        public string Login => Principal.Claims.First(a => a.Type == "Name").Value;
        public UserState State => Enum.Parse<UserState>(Principal.Claims.First(a => a.Type == "State").Value);
        public string Email => Principal.Claims.First(a => a.Type == "Email").Value;        

        public static List<Claim> CreateCustomUserClaims(string login, UserState userState, string email)
        {
            return new List<Claim>
            {
                new("Name", login),
                new("State", userState.ToString()),
                new("Email", email)              
            };
        }
    }
}
