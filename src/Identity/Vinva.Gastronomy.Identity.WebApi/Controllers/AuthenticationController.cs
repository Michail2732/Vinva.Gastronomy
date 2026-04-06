using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Identity.Application.Common;
using Vinva.Gastronomy.Identity.Application.Usecases.Authentication.GetCurrentUser;
using Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Login;
using Vinva.Gastronomy.Identity.Application.Usecases.Authentication.Logout;
using Vinva.Gastronomy.Identity.Application.Usecases.Authentication.RefreshJwtToken;
using Vinva.Gastronomy.Identity.WebApi.Dtos;
using Vinva.Gastronomy.Identity.WebApi.Services;

namespace Vinva.Gastronomy.Identity.WebApi.Controllers
{

    [ApiController]
    [Route("api/Authentication")]
    [Authorize]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthCookieOptionsFactory _cookieOptsFactory;
        private readonly IMediator _mediator;

        public const string ACCESS_TOKEN_KEY = "accessToken";
        public const string REFRESH_TOKEN_KEY = "refreshToken";


        public AuthenticationController(IMediator mediator, AuthCookieOptionsFactory cookieOptsFactory)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _cookieOptsFactory = cookieOptsFactory ?? throw new ArgumentNullException(nameof(cookieOptsFactory));
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
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<UserInfoDto> Login([FromBody]LoginRequest request)
        {
            var result = await _mediator.Send(request);
            SetAccessRefreshToken(result.AccessToken, result.RefreshToken);
            return result.Copy();            
        }

        /// <summary>
        ///     Обновление токена.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Обновляет access токен используя refresh токен.
        /// </remarks>
        [HttpPost("RefreshToken")]
        public async Task<UserInfoDto> RefreshToken()
        {
            var tokens = GetAccessAndRefreshTokens();
            var request = new RefreshTokenRequest
            {
                RefreshToken = tokens.RefreshToken
            };
            var result = await _mediator.Send(request);

            SetAccessRefreshToken(result.AccessToken, result.RefreshToken);
            return result.Copy();
        }        

        /// <summary>
        ///     Получение информации о текущем пользователе.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        [HttpGet("Me")]
        public async Task<UserInfoDto> Me()
        {
            var tokens = GetAccessAndRefreshTokens();
            var query = new GetCurrentUserQuery
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            };
            var result = await _mediator.Send(query);
            return result;
        }

        /// <summary>
        ///     Выход из системы.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task Logout()
        {
            var tokens = GetAccessAndRefreshTokens();
            var logoutRequest = new LogoutRequest
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            };
            await _mediator.Send(logoutRequest);
            DeleteAccessRefreshTokens();
        }

        private (string AccessToken, string RefreshToken) GetAccessAndRefreshTokens()
        {
            if (!Request.Cookies.TryGetValue(ACCESS_TOKEN_KEY, out var accessToken) || string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedException();
            if (!Request.Cookies.TryGetValue(REFRESH_TOKEN_KEY, out var refreshToken) || string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedException();
            return (accessToken, refreshToken);
        }   
        
        private void SetAccessRefreshToken(string? accessToken, string? refreshToken)
        {
            var cookieOptions = _cookieOptsFactory.Create();
            if (!string.IsNullOrEmpty(accessToken))
                Response.Cookies.Append(ACCESS_TOKEN_KEY, accessToken, cookieOptions);
            if (!string.IsNullOrEmpty(refreshToken))
                Response.Cookies.Append(REFRESH_TOKEN_KEY, refreshToken, cookieOptions);
        }

        private void DeleteAccessRefreshTokens(bool deleteAccess = true, bool deleteRefresh = true)
        {
            var cookieOptions = _cookieOptsFactory.Create();
            if (deleteAccess)
                Response.Cookies.Delete(ACCESS_TOKEN_KEY, cookieOptions);
            if (deleteRefresh)
                Response.Cookies.Delete(REFRESH_TOKEN_KEY, cookieOptions);
        }        
    }
}
