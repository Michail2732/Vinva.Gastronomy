using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Domain.Exceptions;
using Vinva.Gastronomy.Identity.Domain.Services;
using PasswordHasher = BCrypt.Net.BCrypt;

namespace Vinva.Gastronomy.Identity.Application.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        private readonly PasswordConfig _settings;

        public PasswordHashService(IOptions<PasswordConfig> options)
        {
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Пароль не может быть пустым", nameof(password));            

            return PasswordHasher.HashPassword(password, _settings.WorkFactor);
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return false;

            try
            {
                return PasswordHasher.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < _settings.MinimumLength)
                return false;

            if (_settings.RequireUppercase && !password.Any(char.IsUpper))
                return false;

            if (_settings.RequireLowercase && !password.Any(char.IsLower))
                return false;

            if (_settings.RequireDigits && !password.Any(char.IsDigit))
                return false;

            if (_settings.RequireSpecialCharacters && !password.Any(c => !char.IsLetterOrDigit(c)))
                return false;

            return true;
        }

        public string GetPasswordRequirements()
        {
            var requirements = new List<string>
        {
            $"минимум {_settings.MinimumLength} символов"
        };

            if (_settings.RequireUppercase)
                requirements.Add("заглавные буквы");

            if (_settings.RequireLowercase)
                requirements.Add("строчные буквы");

            if (_settings.RequireDigits)
                requirements.Add("цифры");

            if (_settings.RequireSpecialCharacters)
                requirements.Add("специальные символы");

            return string.Join(", ", requirements);
        }
    }
}
