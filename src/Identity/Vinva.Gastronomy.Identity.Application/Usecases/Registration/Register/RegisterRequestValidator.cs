using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Vinva.Gastronomy.Identity.Application.Authentication.Login;
using Vinva.Gastronomy.Identity.Domain.Constants;

namespace Vinva.Gastronomy.Identity.Application.Usecases.Registration.Register
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Login)
           .NotEmpty()
           .WithMessage("Логин обязателен")
           .MinimumLength(EntityConstrains.MinLoginLength)
           .WithMessage($"Логин должен содержать минимум {EntityConstrains.MinLoginLength} символов")
           .MaximumLength(EntityConstrains.MaxLoginLength)
           .WithMessage($"Логин не может превышать {EntityConstrains.MaxLoginLength} символов");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Пароль обязателен");
        }

    }
}
