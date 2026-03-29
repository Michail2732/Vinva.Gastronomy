using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Registration.Register
{
    public readonly record struct RegisterCommand : IRequest<RegisterCommandResponce>
    {        
        public string Password { get; init; }
        public string Login { get; init; }
        public string Email { get; init; }
    }
}
