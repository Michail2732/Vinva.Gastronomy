using System;
using System.Collections.Generic;
using System.Linq;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    public sealed class RecipeIngredient : EntityOfT<Guid>
    {
        private readonly List<Quantity> quantities = new();

        public Guid RecipeId { get; internal set; }
        public string Name { get; private set; }
        public Guid IngredientsSetId  { get; private set; }
        public IReadOnlyCollection<Quantity> Quantities => quantities;

#pragma warning disable CS8618
        private RecipeIngredient() :base(GenerateGuid()) { }
#pragma warning restore CS8618

        public RecipeIngredient(Guid id, string name, Guid ingredientsSetId, IEnumerable<Quantity>? quantities = null)
            :base(id)
        {
            IngredientsSetId = ingredientsSetId;
            Name = NormalizeName(name);            
            if (quantities is not null)
                this.quantities.AddRange(quantities);
        }

        public RecipeIngredient(Guid id, Guid recipeId, string name, Guid ingredientsSetId, IEnumerable<Quantity>? quantities = null)
            : this(id, name, ingredientsSetId, quantities)
        {
            RecipeId = recipeId;
        }

        public void Rename(string name)
        {
            Name = NormalizeName(name);
        }

        public void ReplaceQuantities(IEnumerable<Quantity> values)
        {
            quantities.Clear();
            quantities.AddRange(values ?? throw new ArgumentNullException(nameof(values)));
        }

        public void AddQuantity(decimal amount, string unit)
        {
            quantities.Add(new Quantity(amount, unit));
        }        

        private static string NormalizeName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            return name.Trim();
        }
    }
}
