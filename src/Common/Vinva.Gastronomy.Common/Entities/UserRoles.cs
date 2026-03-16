using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Entities
{
    public class UserRoles
    {
        public const string Manager = nameof(UserRole.Manager);
        public const string Administrator = nameof(UserRole.Admin);
        public const string Client = nameof(UserRole.Client);
    }
}
