using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Identity.Application.Constants
{
    public class IdentityApplicationErrors
    {
        /// <summary>
        /// Неверные учетные данные
        /// </summary>
        public static Error InvalidCredentials => new("Identity.InvalidCredentials", "Неверные учетные данные");
        /// <summary>
        /// Пользователь не найден
        /// </summary>
        public static Error UserNotFound => new("Identity.UserNotFound", "Пользователь не найден");        
        /// <summary>
        /// Сотрудник не найден
        /// </summary>
        public static Error EmployeeNotFound => new("Identity.EmployeeNotFound", "Сотрудник не найден");
        /// <summary>
        /// Логин уже существует
        /// </summary>
        public static Error LoginAlreadyExists => new("Identity.LoginAlreadyExists", "Логин уже существует");
        /// <summary>
        /// Токен недействителен
        /// </summary>
        public static Error TokenInvalid => new("Identity.TokenInvalid", "Токен недействителен");
        /// <summary>
        /// Токен истек
        /// </summary>
        public static Error TokenExpired => new("Identity.TokenExpired", "Токен истек");
        /// <summary>
        /// Неверный пароль
        /// </summary>
        public static Error InvalidPassword => new("Identity.InvalidPassword", "Неверный пароль");
        /// <summary>
        /// Неверный текущий пароль
        /// </summary>
        public static Error CurrentPasswordIncorrect => new("Identity.CurrentPasswordIncorrect", "Неверный текущий пароль");
        /// <summary>
        /// Недействительный refresh токен
        /// </summary>
        public static Error RefreshTokenInvalid => new("Identity.RefreshTokenInvalid", "Недействительный refresh токен");
        /// <summary>
        /// Пользователь заблокирован
        /// </summary>
        public static Error UserIsBlocked => new("Identity.UserIsBlocked", "Пользователь заблокирован");
        /// <summary>
        /// Ошибка валидации
        /// </summary>
        public static Error ValidationFailed => new("Identity.ValidationFailed", "Ошибка валидации");
        /// <summary>
        /// ПОльзователь с таким Email уже существует
        /// </summary>
        public static Error UserWithSameEmailExists => new("Identity.UserWithSameEmailExists", "Пользователь с таким email уже существует");
        /// <summary>
        /// Ошибка при отправке письма для регистрации
        /// </summary>
        public static Error CantSendRegistrationMessage => new("Identity.CantSendRegistrationMessage", "Не удалось отправить сообщение на указанный адресс");
        /// <summary>
        /// Некорректный токен регистрации
        /// </summary>
        public static Error IncorrectRegisterToken => new("Identity.IncorrectRegisterToken", "Некорректный токен регистрации");

    }
}
