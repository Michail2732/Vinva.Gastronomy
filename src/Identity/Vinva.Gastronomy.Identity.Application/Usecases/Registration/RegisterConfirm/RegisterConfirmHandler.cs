using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Emails;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Identity.Application.Common.Constants;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;
using Vinva.Gastronomy.Identity.Persistence.Repositories;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Registration.RegisterConfirm
{
    public class RegisterConfirmHandler : IRequestHandler<RegisterConfirmRequest, Result>
    {
        private readonly IUsersRepository _usersRepository;        
        private readonly IRegistrationService _registrationService;

        public RegisterConfirmHandler(IRegistrationService registrationService,
            IUsersRepository usersRepository)
        {                        
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public async Task<Result> Handle(RegisterConfirmRequest request, CancellationToken cancellationToken)
        {
            var token = await _registrationService.PopTokenAsync(request.TokenId);
            if (token == null)
                return IdentityApplicationErrors.IncorrectRegisterToken;

            var user = new User(token.Login, token.PasswordHash, token.Email);
            await _usersRepository.AddAsync(user);

            return Result.Success();
        }
    }
}
