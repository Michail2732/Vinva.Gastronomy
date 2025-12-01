using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Common.Infrastructure.Validations
{
    public static class ValidationExtensions
    {        
        public static Result<T> HandleValidationErrors<T>(this ValidationResult validationResult)
        {
            var error = validationResult.Errors.FirstOrDefault();
            return Result.Failure<T>(error == null
                ? Error.NotDefined
                : new Error(error.ErrorCode, error.ErrorMessage));
        }

        public static Result HandleValidationErrors(this ValidationResult validationResult)
        {
            var error = validationResult.Errors.FirstOrDefault();
            return Result.Failure(error == null
                ? Error.NotDefined
                : new Error(error.ErrorCode, error.ErrorMessage));
        }

    }
}
