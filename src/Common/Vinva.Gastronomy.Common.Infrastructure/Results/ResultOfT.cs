using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Infrastructure.Results
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        [JsonConstructor]
        protected internal Result()
            : base()
        {
            _value = default;
        }

        [JsonPropertyName("value")]
        public TValue? Value
        {
            get => IsSuccess ? _value! : default;
            init => _value = value;
        }

        public static implicit operator Result<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }
}
