using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Domain.Services;

namespace Vinva.Gastronomy.Identity.Application.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordConfig _settings;
        private readonly IHashService _hashService;

        public PasswordService(IOptions<PasswordConfig> options, IHashService hashService)
        {
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
        }

        public string HashPassword(string password) => _hashService.Hash(password);        

        public bool VerifyPassword(string password, string hash) => _hashService.Verify(password, hash);
        

        public bool IsValidPassword(string password, out string? errorMessage)
        {
            errorMessage = null;
            var errorBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(password))
            {
                errorBuilder.Append("Пароль не может быть пустым");
                return false;
            }

            if (password.Length < _settings.MinimumLength)
            {
                errorBuilder.Append($"Пароль должен содержать минимум {_settings.MinimumLength} символов");
                return false;
            }

            if (_settings.RequireUppercase && !password.Any(char.IsUpper))
            {
                errorBuilder.Append("Пароль должен содержать хотя бы одну заглавную букву");
                return false;
            }

            if (_settings.RequireLowercase && !password.Any(char.IsLower))
            {
                errorBuilder.Append("Пароль должен содержать хотя бы одну строчную букву");
                return false;
            }

            if (_settings.RequireDigits && !password.Any(char.IsDigit))
            {
                errorBuilder.Append("Пароль должен содержать хотя бы одну цифру");
                return false;
            }

            if (_settings.RequireSpecialCharacters && !password.Any(c => !char.IsLetterOrDigit(c)))
            {
                errorBuilder.Append("Пароль должен содержать хотя бы один специальный символ");
                return false;
            }

            errorMessage = errorBuilder.ToString();
            return errorBuilder.Length == 0;
        }               
    }
}
