using System;
using System.Collections.Generic;
using System.Linq;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    public sealed class RecipeIngredient : Entity, IEquatable<RecipeIngredient?>
    {
        private readonly List<RecipeIngredientQuantity> quantities = new();

        public Guid RecipeId { get; internal set; }
        public string Name { get; private set; }
        public string? Part { get; private set; }
        public IReadOnlyCollection<RecipeIngredientQuantity> Quantities => quantities;

#pragma warning disable CS8618
        private RecipeIngredient() { }
#pragma warning restore CS8618

        public RecipeIngredient(string name, IEnumerable<RecipeIngredientQuantity>? quantities = null, string? part = null)
        {
            Name = NormalizeName(name);
            SetPart(part);
            if (quantities is not null)
                this.quantities.AddRange(quantities);
        }

        public RecipeIngredient(Guid recipeId, string name, IEnumerable<RecipeIngredientQuantity>? quantities = null, string? part = null)
            : this(name, quantities, part)
        {
            RecipeId = recipeId;
        }

        public void SetPart(string? part)
        {
            Part = string.IsNullOrWhiteSpace(part) ? null : part.Trim();
        }

        public void Rename(string name)
        {
            Name = NormalizeName(name);
        }

        public void ReplaceQuantities(IEnumerable<RecipeIngredientQuantity> values)
        {
            quantities.Clear();
            quantities.AddRange(values ?? throw new ArgumentNullException(nameof(values)));
        }

        public void AddQuantity(decimal amount, string unit)
        {
            quantities.Add(new RecipeIngredientQuantity(amount, unit));
        }

        public bool Equals(RecipeIngredient? other)
        {
            return other is not null && RecipeId == other.RecipeId && Name == other.Name;
        }

        public override bool Equals(object? obj) => Equals(obj as RecipeIngredient);
        public override bool Equals(IEntity? obj) => Equals(obj as RecipeIngredient);
        public override int GetHashCode() => HashCode.Combine(RecipeId, Name);

        private static string NormalizeName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            return name.Trim();
        }
    }
}
