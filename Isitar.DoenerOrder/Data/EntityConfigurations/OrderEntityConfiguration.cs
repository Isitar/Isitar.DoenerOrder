using Isitar.DoenerOrder.Domain;
using Isitar.DoenerOrder.Domain.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isitar.DoenerOrder.Data.EntityConfigurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Product)
                .WithMany()
                .IsRequired(true);
            builder.HasOne(x => x.User)
                .WithMany()
                .IsRequired(true);
            builder.HasMany(x => x.OrderLines)
                .WithOne(x => x.Order);
        }
    }
}