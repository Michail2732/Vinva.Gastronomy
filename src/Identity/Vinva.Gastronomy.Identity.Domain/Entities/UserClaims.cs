using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Domain.Entities
{
    public class UserClaims
    {
        public const string StateType = "State";


        public ClaimsPrincipal Principal { get; }

        public string Login => Principal.Claims.First(a => a.Type == ClaimTypes.Name).Value;

        public UserState State => Enum.Parse<UserState>(Principal.Claims.First(a => a.Type == StateType).Value);

        public string Email => Principal.Claims.First(a => a.Type == ClaimTypes.Email).Value;

        public string Role => Principal.Claims.First(a => a.Type == ClaimTypes.Role).Value;


        public UserClaims(ClaimsPrincipal principal)
        {
            Principal = principal ?? throw new ArgumentNullException(nameof(principal));
        }        


        public static List<Claim> CreateClaims(User user)
        {
            return new List<Claim>
            {
                new(ClaimTypes.Name, user.Login),
                new(StateType, user.State.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString()),
            };
        }
    }
}
