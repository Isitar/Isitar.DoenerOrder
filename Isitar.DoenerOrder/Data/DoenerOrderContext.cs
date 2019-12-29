using Isitar.DoenerOrder.Data.EntityConfigurations;
using Isitar.DoenerOrder.Domain;
using Isitar.DoenerOrder.Domain.DAO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Data
{
    public class DoenerOrderContext : IdentityDbContext<User, Role, int>
    {
        public DoenerOrderContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<BulkOrder> BulkOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductIngredient> ProductIngredients { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new BulkOrderEntityConfiguration());
            builder.ApplyConfiguration(new IngredientEntityConfiguration());
            builder.ApplyConfiguration(new OrderEntityConfiguration());
            builder.ApplyConfiguration(new OrderLineEntityConfiguration());
            builder.ApplyConfiguration(new ProductEntityConfiguration());
            builder.ApplyConfiguration(new ProductIngredientEntityConfiguration());
            builder.ApplyConfiguration(new SupplierEntityConfiguration());
        }
    }
}