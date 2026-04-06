using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.WebApi.Services
{
    public class AuthCookieOptionsFactory
    {
        private readonly IHostEnvironment _env;

        public AuthCookieOptionsFactory(IHostEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public CookieOptions Create()
        {
            if (_env.IsDevelopment())
            {
                return new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(2),
                    Path = "/"
                };
            }
            else
            {
                return new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(2),
                    Path = "/"
                };
            }
        }

    }
}
