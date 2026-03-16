using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Registration.RegisterConfirm
{
    public readonly record struct RegisterConfirmCommand : IRequest
    {
        public Guid TokenId { get; init; }
    }
}
