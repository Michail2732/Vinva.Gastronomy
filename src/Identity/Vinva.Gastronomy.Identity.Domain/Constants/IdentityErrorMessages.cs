using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Domain.Constants
{
    public class IdentityErrorMessages
    {
        public static string NewEmailIsIncorrect(string newEmail) => $"Email '{newEmail}' некорректный";
        public static string RegistrationTokenNotFound(Guid id) => $"Не удалось найти токен регистрации '{id}'";
        public static string RegistrationTokenHasExpired(Guid id) => $"Срок действия токена регистрации '{id}' истёк";
        public static string ConfirmMessageAlreadySent(Guid tokenId) => $"Для токена регистрации '{tokenId}' письмо уже было отправлено";
        public static string RegistrationTokenAlreadyComplete(Guid tokenId) => $"Токен регистрации '{tokenId}' уже завершён";
    }
}
