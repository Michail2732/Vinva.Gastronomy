using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Validations;
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

        public async Task<RefreshTokenResponce> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var validator = new RefreshTokenRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.HandleValidationErrors<RefreshTokenResponce>().Error;
                throw new BadRequestException(errors);
            }                
            
            var userTokens = await _dbContext.UserTokens.FirstOrDefaultAsync(a => a.RefreshToken == request.RefreshToken, cancellationToken);

            if (userTokens == null)
                throw new UnauthorizedException(IdentityApplicationErrors.RefreshTokenInvalid);

            var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == userTokens.UserId, cancellationToken);

            if (user == null)
                throw new BadRequestException(IdentityApplicationErrors.UserCouldNotFound);

            if (user.State == UserState.Blocked)
                throw new BadRequestException(IdentityApplicationErrors.RefreshTokenInvalid);

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user, cancellationToken);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user, cancellationToken);
            var expiresAt = await _tokenService.GetTokenExpirationAsync(accessToken, cancellationToken);

            userTokens.SetNewToken(accessToken, refreshToken, expiresAt);
            _dbContext.UserTokens.Update(userTokens);
            await _dbContext.SaveChangesAsync();

            return new RefreshTokenResponce
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                Login = user.Login,
                Role = user.Role
            };
        }
    }
}
