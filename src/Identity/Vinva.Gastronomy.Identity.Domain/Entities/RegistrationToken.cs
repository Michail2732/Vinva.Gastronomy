using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;

namespace Vinva.Gastronomy.Identity.Domain.Entities
{
    public class RegistrationToken : EntityOfT<Guid>
    {
        public DateTimeOffset ExpiresAt { get; private set; }
        public string PasswordHash { get; private set; }        
        public string Login { get; private set; }
        public string Email { get; private set; }

        public RegistrationToken(DateTimeOffset expiresAt,
            string passwordHash, string login, string email)
        {            
            ExpiresAt = expiresAt;
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));            
            Login = login ?? throw new ArgumentNullException(nameof(login));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public bool IsExpires(TimeProvider timeProvider)
        {
            return timeProvider.GetUtcNow() > ExpiresAt;
        }
    }
}
