using Isitar.DoenerOrder.Core.Data.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isitar.DoenerOrder.Core.Data.EntityConfigurations
{
    internal class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Product)
                .WithMany()
                .IsRequired();
            builder.HasOne(x => x.User)
                .WithMany()
                .IsRequired();
            builder.HasMany(x => x.OrderLines)
                .WithOne(x => x.Order);
        }
    }
}