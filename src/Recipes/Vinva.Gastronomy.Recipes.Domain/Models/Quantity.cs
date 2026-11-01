using System;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    public sealed record Quantity
    {
        public decimal Amount { get; init; }
        public string Unit { get; init; }

        public Quantity(decimal amount, string unit)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Ingredient amount must be greater than zero.");

            ArgumentException.ThrowIfNullOrWhiteSpace(unit);
            Amount = amount;
            Unit = unit.Trim();
        }
    }
}
