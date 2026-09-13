using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Domain.Services
{
    public interface IHashService
    {
        /// <summary>
        /// Хеширование данных
        /// </summary>
        /// <param name="originalData">Исходные данные</param>
        /// <returns>Хешированные данные</returns>
        /// <exception cref="IdentityDomainException">В случае если не удалось сформировать hash</exception>
        string Hash(string originalData);

        /// <summary>
        /// Проверка данных
        /// </summary>
        /// <param name="originalData">Исходные данные</param>
        /// <param name="hash">Хеш для сравнения</param>
        /// <returns>True, если данные соответствуют хешу</returns>
        bool Verify(string originalData, string hash);
    }
}