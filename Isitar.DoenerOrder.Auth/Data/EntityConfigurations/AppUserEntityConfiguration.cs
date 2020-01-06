using Isitar.DoenerOrder.Auth.Data.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isitar.DoenerOrder.Auth.Data.EntityConfigurations
{
    public class AppUserEntityConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.UserId)
                .IsRequired();
        }
    }
}