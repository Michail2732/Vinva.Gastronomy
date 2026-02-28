using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;

namespace Vinva.Gastronomy.Common.Modularity.MediatR
{
    public class ValidationMediatRBehavior<TRequest, TResponce> : IPipelineBehavior<TRequest, TResponce>
        where TRequest : notnull
    {
        private readonly IList<IValidator<TRequest>> _validators;

        public ValidationMediatRBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators?.ToList() ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponce> Handle(TRequest request, RequestHandlerDelegate<TResponce> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validatorsCount = _validators.Count();

                ValidationResult[] validResults = new ValidationResult[validatorsCount];

                for (int i = 0; i < validatorsCount; i++)
                {
                    validResults[i] = await _validators[i].ValidateAsync(context, cancellationToken);
                }

                var failures = validResults.Where(r => r.Errors.Count > 0)
                                           .SelectMany(r => r.Errors)
                                           .ToList();

                if (failures.Count > 0)
                {
                    var fluentEx = new ValidationException(failures);
                    throw new BadRequestException(fluentEx.Message, fluentEx);
                }                    
            }

            return await next();
        }
    }
}
