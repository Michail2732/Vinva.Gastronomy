using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.RefreshJwtToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponce>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly ITokenService _tokenService;        

        public RefreshTokenHandler(IdentityDbContext dbContext, 
            ITokenService tokenService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));            
        }

        public async Task<RefreshTokenResponce> Handle(RefreshTokenRequest request, CancellationToken ct)
        {
            var query = from tokenQ in _dbContext.UserTokens
                        where tokenQ.RefreshToken == request.RefreshToken
                        join userQ in _dbContext.Users on tokenQ.UserId equals userQ.Id into userJoin
                        from userQ in userJoin.DefaultIfEmpty()
                        select new { Tokens = tokenQ, User = userQ };

            var result = await query.FirstOrDefaultAsync(ct);
            var tokens = result?.Tokens;
            var user = result?.User;

            if (tokens == null)
                throw new UnauthorizedException(IdentityApplicationErrors.RefreshTokenInvalid);            

            if (user == null)
                throw new BadRequestException(IdentityApplicationErrors.UserCouldNotFound);

            if (user.State == UserState.Blocked)
                throw new BadRequestException(IdentityApplicationErrors.RefreshTokenInvalid);

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user, ct);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user, ct);            

            tokens.SetNewToken(accessToken, refreshToken);
            _dbContext.UserTokens.Update(tokens);
            await _dbContext.SaveChangesAsync();

            return new RefreshTokenResponce
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,                
                Login = user.Login,
                Roles = user.Roles,
                Email = user.Email,
                Id = user.Id,
                State = user.State
            };
        }
    }
}
