using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public readonly record struct IngredientQuantities: IEnumerable<IngredientQuantity>
    {
        private readonly IngredientQuantity[] _rawData;        

        public IngredientQuantities(IEnumerable<IngredientQuantity> items)
        {
            _rawData = items.ToArray();
            if (_rawData.Length > _rawData.DistinctBy(a=> a.Measure).Count())
                throw new RecipeDomainException(typeof(IngredientQuantities), RecipeDomainErrors.IngredientQuantitiesHasDuplicate(ToString()));
        }

        public bool IsEmpty => _rawData.Length == 0;

        public static IngredientQuantities Parse(string str)
        {
            var rawItems = str.Trim().Split(';');
            var quantities = new IngredientQuantity[rawItems.Length];
            for (int i = 0; i < rawItems.Length; i++)
            {
                quantities[i] = IngredientQuantity.Parse(rawItems[i]);
            }
            return new IngredientQuantities(quantities);
        }

        public static implicit operator IngredientQuantities(string str)
        {
            return Parse(str);
        }

        public static implicit operator string(IngredientQuantities val)
        {
            return val.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _rawData)
            {
                sb.Append(item.ToString()+';');
            }
            return sb.ToString();
        }

        public IEnumerator<IngredientQuantity> GetEnumerator() => _rawData.AsEnumerable().GetEnumerator();        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
