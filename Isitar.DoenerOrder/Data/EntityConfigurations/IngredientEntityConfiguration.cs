using Isitar.DoenerOrder.Domain;
using Isitar.DoenerOrder.Domain.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isitar.DoenerOrder.Data.EntityConfigurations
{
    public class IngredientEntityConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Label).IsRequired(true);
        }
    }
}