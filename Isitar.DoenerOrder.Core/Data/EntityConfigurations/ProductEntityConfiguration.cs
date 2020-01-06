using Isitar.DoenerOrder.Domain;
using Isitar.DoenerOrder.Domain.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isitar.DoenerOrder.Data.EntityConfigurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Label).IsRequired(true);
            builder.Property(x => x.Price).IsRequired(true);
            builder.HasOne(x => x.Supplier)
                .WithMany(x => x.Products)
                .IsRequired(true);
        }
    }
}