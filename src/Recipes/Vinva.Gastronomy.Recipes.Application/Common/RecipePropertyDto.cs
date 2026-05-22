using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public class RecipePropertyDto
    {
        public required string Name { get; init; }
        public required List<string> Values { get; init; }        
    }
}
