using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Authentication.ValidateJwtToken
{
    public readonly record struct ValidateTokenRequest : IRequest
    {
        public string AccessToken { get; init; }
    }
}
