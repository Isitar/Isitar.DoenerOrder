using Isitar.DoenerOrder.Core.Data.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isitar.DoenerOrder.Core.Data.EntityConfigurations
{
    public class OrderLineEntityConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderLines)
                .IsRequired(true);
            builder.HasOne(x => x.Ingredient)
                .WithMany()
                .IsRequired(true);
        }
    }
}