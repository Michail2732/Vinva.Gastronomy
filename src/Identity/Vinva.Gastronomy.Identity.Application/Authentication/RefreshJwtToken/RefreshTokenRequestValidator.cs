using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Vinva.Gastronomy.Identity.Application.Authentication.Login;

namespace Vinva.Gastronomy.Identity.Application.Authentication.RefreshJwtToken
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
           .NotEmpty();           
        }

    }
}
