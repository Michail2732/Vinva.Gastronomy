using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Entities
{
    public class UserInfo 
    {
        public required Guid Id { get; init; }
        public required string Login { get; init; }
        public required string Email { get; init; }
        public UserRole[] Roles { get; init; } = new UserRole[0];
        public required UserState State { get; init; }        
    }
}
