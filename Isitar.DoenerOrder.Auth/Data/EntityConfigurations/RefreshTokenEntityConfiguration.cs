using Isitar.DoenerOrder.Auth.Data.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Isitar.DoenerOrder.Auth.Data.EntityConfigurations
{
    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Token).IsRequired(true);
            builder.Property(x => x.JwtTokenId).IsRequired(true);
            builder.Property(x => x.Expires).IsRequired(true);
            builder.Property(x => x.Used).IsRequired(true);
            builder.Property(x => x.Invalidated).IsRequired(true);
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}