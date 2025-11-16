using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Utilities;
using Vinva.Gastronomy.Identity.Domain.Constants;
using Vinva.Gastronomy.Identity.Domain.Exceptions;

namespace Vinva.Gastronomy.Identity.Domain.Entities
{
    public class User : EntityOfT<Guid>
    {
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; set; }
        public UserState State { get; set; }        
        public DateTimeOffset LastLoginAt { get; set; }
        public DateTimeOffset LastLogoutAt { get; set; }
        public DateTimeOffset LastPasswordChangedAt { get; private set; }
        public DateTimeOffset LastEmailChangedAt { get; private set; }

        public User(string login, string passwordHash, string email)
        {
            Login = login ?? throw new ArgumentNullException(nameof(login));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
        
        public void ChangePassword(string newPasswordHash, DateTimeOffset currentTime)
        {
            PasswordHash = newPasswordHash;
            LastPasswordChangedAt = currentTime;
        }


        public void ChangeEmail(string newEmail, DateTimeOffset currentTime)        
        {
            if (!EmailUtility.Check(newEmail))
                throw new IdentityDomainException(GetType(),
                    IdentityErrorMessages.NewEmailIsIncorrect(newEmail));
            Email = newEmail;
            LastEmailChangedAt = currentTime;
        }


    }
}
