using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Identity.Application.Common
{
    public record UserInfoDto
    {
        public required Guid Id { get; init; }
        public required string Login { get; init; }
        public required string Email { get; init; }
        public required UserState State { get; init; }
        public required UserRole[] Roles { get; init; }

        public UserInfoDto Copy() => new UserInfoDto
        {
            Id = Id,
            Email = Email,
            Login = Login,
            Roles = Roles.ToArray(),
            State = State
        };
    }
}
