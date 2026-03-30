using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Exceptions;
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
        private readonly TimeProvider _timeProvider;

        public RegisterConfirmCommandHandler(IRegistrationService registrationService,
            IdentityDbContext usersRepository,
            TimeProvider timeProvider)
        {
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _dbContext = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public async Task Handle(RegisterConfirmCommand request, CancellationToken cancellationToken)
        {
            var token = await _dbContext.RegistrationTokens.FirstOrDefaultAsync(a => a.Id == request.TokenId);            
            if (token == null)
                throw new BadRequestException(IdentityApplicationErrors.IncorrectRegisterToken);

            if (token.IsConfirmCompleate)
                throw new BadRequestException(IdentityApplicationErrors.RegistrationTokenAlreadyComplete);

            if (token.IsExpires(_timeProvider))
                throw new BadRequestException(IdentityApplicationErrors.RegistrationTokenIsExpired);

            var user = new User(token.Login, token.PasswordHash, token.Email);
            token.Confirm(_timeProvider);

            _dbContext.RegistrationTokens.Update(token);
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
