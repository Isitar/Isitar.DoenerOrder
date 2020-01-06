using Isitar.DoenerOrder.Core.Data.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isitar.DoenerOrder.Core.Data.EntityConfigurations
{
    public class BulkOrderEntityConfiguration : IEntityTypeConfiguration<BulkOrder>
    {
        public void Configure(EntityTypeBuilder<BulkOrder> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.CreationUser).WithMany().IsRequired(true);
            builder.Property(x => x.Deadline).IsRequired(true);
            builder.HasOne(x => x.Supplier).WithMany().IsRequired(true);
        }
    }
}