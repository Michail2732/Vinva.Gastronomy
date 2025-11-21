using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Domain.Services
{
    /// <summary>
    /// Cервис для работы с токенами аутентификации
    /// </summary>
    internal interface ITokenService
    {
        /// <summary>
        /// Генерация токена доступа для пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="storageIds">Список доступных складов</param>
        /// <returns>Токен доступа</returns>
        Task<string> GenerateAccessTokenAsync(User user);

        /// <summary>
        /// Генерация токена обновления
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Токен обновления</returns>
        Task<string> GenerateRefreshTokenAsync(Guid userId);

        /// <summary>
        /// Валидация токена и извлечение информации о пользователе
        /// </summary>
        /// <param name="token">Токен для валидации</param>
        /// <returns>Информация о пользователе или null если токен невалидный</returns>
        Task<ClaimsPrincipal?> ValidateTokenAsync(string token);

        /// <summary>
        /// Получение времени истечения токена
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns>Время истечения</returns>
        Task<DateTimeOffset> GetTokenExpirationAsync(string token);
    }
}
