using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public class CategoryDbConfiguration : DescriptiveEntityDbConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.ToTable("Categories");

            builder.HasKey(a => a.Id);            
        }
    }
}
