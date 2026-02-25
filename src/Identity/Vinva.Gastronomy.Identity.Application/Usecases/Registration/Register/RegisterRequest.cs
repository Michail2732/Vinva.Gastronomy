using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Registration.Register
{
    public readonly record struct RegisterRequest : IRequest
    {        
        public string Password { get; init; }
        public string Login { get; init; }
        public string Email { get; init; }
    }
}
