using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vinva.Gastronomy.Identity.Domain.Services;
using Hasher = BCrypt.Net.BCrypt;

namespace Vinva.Gastronomy.Identity.Application.Services
{
    public class HashService : IHashService
    {
        private const int WORK_FACTOR = 10;

        public string Hash(string originalData)
        {
             if (string.IsNullOrWhiteSpace(originalData))
                throw new ArgumentNullException("Исходные данные не могут быть пустыми", nameof(originalData));            

            return Hasher.HashPassword(originalData, WORK_FACTOR);
        }

        public bool Verify(string originalData, string hash)
        {
              if (string.IsNullOrWhiteSpace(originalData) || string.IsNullOrWhiteSpace(hash))
                return false;

            try
            {
                return Hasher.Verify(originalData, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}