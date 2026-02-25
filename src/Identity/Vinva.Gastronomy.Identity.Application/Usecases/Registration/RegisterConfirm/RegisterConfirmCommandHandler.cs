using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Registration.RegisterConfirm
{
    public class RegisterConfirmCommandHandler : IRequestHandler<RegisterConfirmCommand>
    {
        private readonly IdentityDbContext _dbContext;        
        private readonly IRegistrationService _registrationService;

        public RegisterConfirmCommandHandler(IRegistrationService registrationService,
            IdentityDbContext usersRepository)
        {                        
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _dbContext = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public async Task Handle(RegisterConfirmCommand request, CancellationToken cancellationToken)
        {
            var token = await _registrationService.PopTokenAsync(request.TokenId);
            if (token == null)
                throw new BadRequestException(IdentityApplicationErrors.IncorrectRegisterToken);

            var user = new User(token.Login, token.PasswordHash, token.Email);
            await _dbContext.Users.AddAsync(user);
        }
    }
}
