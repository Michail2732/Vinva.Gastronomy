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
    public interface ITokenService
    {
        /// <summary>
        /// Генерация токена доступа для пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="storageIds">Список доступных складов</param>
        /// <returns>Токен доступа</returns>
        Task<string> GenerateAccessTokenAsync(User user, CancellationToken ct = default);

        /// <summary>
        /// Генерация токена обновления
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Токен обновления</returns>
        Task<string> GenerateRefreshTokenAsync(User user, CancellationToken ct = default);

        /// <summary>
        /// Добавить JWT токен в чёрный список
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        //Task AddToBlackListAsync(string token, CancellationToken ct = default);

        /// <summary>
        /// Валидация токена и извлечение информации о пользователе
        /// </summary>
        /// <param name="token">Токен для валидации</param>
        /// <returns>Информация о пользователе или null если токен невалидный</returns>
        Task<UserTokenPrincipals?> ValidateTokenAsync(string token, CancellationToken ct = default);

        /// <summary>
        /// Получение времени истечения токена
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns>Время истечения</returns>
        Task<DateTimeOffset> GetTokenExpirationAsync(string token, CancellationToken ct = default);
    }
}
