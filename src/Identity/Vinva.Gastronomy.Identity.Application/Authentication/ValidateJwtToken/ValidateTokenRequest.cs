using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Identity.Application.Authentication.ValidateJwtToken
{
    public readonly record struct ValidateTokenRequest : IRequest<Result>
    {
        public string AccessToken { get; init; }
    }
}
