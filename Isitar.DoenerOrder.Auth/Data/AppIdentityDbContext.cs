using Isitar.DoenerOrder.Auth.Data.DAO;
using Isitar.DoenerOrder.Auth.Data.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Auth.Data
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        
        public AppIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RefreshTokenEntityConfiguration());
        }
    }
}