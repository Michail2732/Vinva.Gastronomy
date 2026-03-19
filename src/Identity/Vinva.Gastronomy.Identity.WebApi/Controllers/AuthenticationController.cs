using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Identity.Application.Usecases.Authentication.GetCurrentUser;
using Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Login;
using Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Logout;
using Vinva.Gastronomy.Identity.Application.Usecases.Authentication.RefreshJwtToken;
using Vinva.Gastronomy.Identity.Application.Usecases.Authentication.ValidateJwtToken;
using Vinva.Gastronomy.Identity.WebApi.Dtos;

namespace Vinva.Gastronomy.Identity.WebApi.Controllers
{

    [ApiController]
    [Route("api/Authentication")]
    [Authorize]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        /// <summary>
        ///     Аутентификация пользователя.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Выполняет аутентификацию пользователя по логину и паролю.
        /// </remarks>
        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<LoginResponceDto> Login([FromBody]LoginRequest request)
        {
            var result = await _mediator.Send(request);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(2),
                Path = "api/Authentication"
            };
            Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);
            return new LoginResponceDto
            {
                AccessToken = result.AccessToken,
                ExpiresAt = result.ExpiresAt,
                Login = result.Login,
                Roles = result.Roles,
            };
        }

        /// <summary>
        ///     Обновление токена.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Обновляет access токен используя refresh токен.
        /// </remarks>
        [HttpPost("/refresh")]
        public async Task<RefreshTokenResponce> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) ||
                string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedException();
            var request = new RefreshTokenRequest
            {
                RefreshToken = refreshToken
            };
            var result = await _mediator.Send(request);
            return result;
        }

        /// <summary>
        ///     Валидация токена
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Проверяет валидность JWT токена.
        /// </remarks>
        [HttpPost("/validate")]
        [AllowAnonymous]
        public async Task ValidateTokenPost([FromBody]ValidateTokenRequest request)
        {
            await _mediator.Send(request);            
        }        

        /// <summary>
        ///     Получение информации о текущем пользователе.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        [HttpGet("/me")]
        public async Task<GetCurrentUserQueryResponse> GetCurrentUser()
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new UnauthorizedException();
            }

            var query = new GetCurrentUserQuery { AccessToken = token };
            var result = await _mediator.Send(query);

            return result;
        }

        /// <summary>
        ///     Выход из системы.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/logout")]
        public async Task Logout([FromBody]LogoutRequest request)
        {            
            await _mediator.Send(request);
        }
    }
}
