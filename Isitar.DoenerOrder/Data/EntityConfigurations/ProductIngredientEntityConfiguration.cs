using Isitar.DoenerOrder.Domain;
using Isitar.DoenerOrder.Domain.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isitar.DoenerOrder.Data.EntityConfigurations
{
    public class ProductIngredientEntityConfiguration : IEntityTypeConfiguration<ProductIngredient>
    {
        public void Configure(EntityTypeBuilder<ProductIngredient> builder)
        {
            builder.HasKey(t => new {t.ProductId, t.IngredientId});
            builder.Property(x => x.AdditionalCost).IsRequired(true);
        }
    }
}