using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Identity.Domain.Exceptions;

namespace Vinva.Gastronomy.Identity.Domain.Services
{
    /// <summary>
    /// Сервис для работы с паролем
    /// </summary>
    public interface IPasswordService
    {
        /// <summary>
        /// Хеширование пароля
        /// </summary>
        /// <param name="password">Исходный пароль</param>
        /// <returns>Хешированный пароль</returns>
        /// <exception cref="IdentityDomainException">В случае если не удалось сформировать hash</exception>
        string HashPassword(string password);

        /// <summary>
        /// Проверка пароля
        /// </summary>
        /// <param name="password">Исходный пароль</param>
        /// <param name="hash">Хеш для сравнения</param>
        /// <returns>True, если пароль соответствует хешу</returns>
        bool VerifyPassword(string password, string hash);

        /// <summary>
        /// Валидация требований к паролю
        /// </summary>
        /// <param name="password">Пароль для проверки</param>
        /// <param name="errorMessage">Сообщение об ошибке, если пароль не соответствует требованиям</param>
        /// <returns>True, если пароль соответствует требованиям</returns>
        bool IsValidPassword(string password, out string? errorMessage);        
    }
}
