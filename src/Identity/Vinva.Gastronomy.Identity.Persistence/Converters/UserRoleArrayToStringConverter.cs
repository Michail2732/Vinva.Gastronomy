using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Identity.Persistence.Converters
{
    public class UserRoleArrayToStringConverter : ValueConverter<UserRole[], string>
    {
        private const char Separator = ',';
        private const int MaxLength = 128;

        public UserRoleArrayToStringConverter()
            : base(roles => ConvertRolesToString(roles), str => ConvertStringToRoles(str))
        {
        }

        private static string ConvertRolesToString(UserRole[] roles)
        {
            if (roles == null || roles.Length == 0)
                return string.Empty;

            
            var result = string.Join(Separator, roles.Select(r => r.ToString()));

            
            if (result.Length > MaxLength)
                throw new InvalidOperationException($"Роли пользователя превышают максимальную длину {MaxLength} символов");

            return result;
        }

        private static UserRole[] ConvertStringToRoles(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return Array.Empty<UserRole>();

            
            return str.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Enum.Parse<UserRole>(s.Trim()))
                .ToArray();
        }
    }

}
