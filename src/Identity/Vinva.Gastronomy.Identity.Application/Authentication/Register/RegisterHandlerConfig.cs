using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Infrastructure.Emails
{
    public class RegisterHandlerConfig
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpCredentialAddress { get; set; }
        public string SmtpCredentialPassword { get; set; }
        public string RegisterUrlTemplate { get; set; }
        public string MailSubject { get; set; }
        public string MailBodyTemplate { get; set; }
        public string BodyTemplateReplacePrefix { get; set; }


        public string GetMailBody(string login, Guid registerTokenId)
        {
            return MailBodyTemplate.Replace(BodyTemplateReplacePrefix + "Login", login)
                .Replace(BodyTemplateReplacePrefix + "RegisterTokenId", registerTokenId.ToString());
        }

    }
}
