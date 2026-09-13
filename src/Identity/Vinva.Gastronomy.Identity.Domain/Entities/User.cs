using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Identity.Domain.Constants;
using Vinva.Gastronomy.Identity.Domain.Exceptions;

namespace Vinva.Gastronomy.Identity.Domain.Entities
{
    public class User : EntityOfT<Guid>
    {
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }
        public string Email { get; private set; }
        public UserRole[] Roles { get; set; } = Array.Empty<UserRole>();
        public UserState State { get; set; }                
        public UserSession? Session { get; set; }        
        public UserRegistration? Registration { get; set; }        
        
 
#pragma warning disable CS8618
        private User() : base(GetDefaultGuid()) { }
#pragma warning restore CS8618 

        public User(Guid id,string login, string passwordHash, string email) : base(id)
        {
            Login = login ?? throw new ArgumentNullException(nameof(login));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Email = email ?? throw new ArgumentNullException(nameof(email));         
        }

        public User(string login, string passwordHash, string email) : this(GenerateGuid(), login, passwordHash, email)
        {
        }


        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;            
        }


        public void ChangeEmail(string newEmail)
        {
            if (!EmailService.Check(newEmail))
                throw new IdentityDomainException(IdentityErrorMessages.NewEmailIsIncorrect(newEmail));
            Email = newEmail;            
        }        

    }
}
