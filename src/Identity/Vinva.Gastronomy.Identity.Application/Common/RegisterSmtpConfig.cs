using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Application.Common
{
    public sealed class RegisterSmtpConfig
    {
        public string SmtpHost { get; init; } = string.Empty;
        public int SmtpPort { get; init; }
        public string SmtpCredentialAddress { get; init; } = string.Empty;
        public string SmtpCredentialPassword { get; init; } = string.Empty;        
        public string MailSubject { get; init; } = string.Empty;
        public string MailBodyTemplate { get; init; } = string.Empty;        


        public string GetMailBody(string login, Uri confirmUri)
        {
            return string.Format(MailBodyTemplate, login, confirmUri.ToString());
        }
    }
}
