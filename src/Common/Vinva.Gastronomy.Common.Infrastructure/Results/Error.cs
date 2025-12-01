using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Infrastructure.Results
{
    public sealed record Error
    {
        [JsonPropertyName("code")]
        public string Code { get; init; }

        [JsonPropertyName("description")]
        public string Description { get; init; }

        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
        public static implicit operator Result(Error error) => Result.Failure(error);

        public static readonly Error NotDefined = new("Error.NotDefined", "Не описанная ошибка");
    }
}
