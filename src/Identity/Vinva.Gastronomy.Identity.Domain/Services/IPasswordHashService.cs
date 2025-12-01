using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Domain.Services
{
    /// <summary>
    /// Сервис для работы с хешированием паролей
    /// </summary>
    public interface IPasswordHashService
    {
        /// <summary>
        /// Хеширование пароля
        /// </summary>
        /// <param name="password">Исходный пароль</param>
        /// <returns>Хешированный пароль</returns>
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
        /// <returns>True, если пароль соответствует требованиям</returns>
        bool IsValidPassword(string password);

        /// <summary>
        /// Получение требований к паролю в виде текста
        /// </summary>
        /// <returns>Описание требований к паролю</returns>
        string GetPasswordRequirements();
    }
}
