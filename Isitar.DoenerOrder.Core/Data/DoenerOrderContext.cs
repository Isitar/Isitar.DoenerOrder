using Isitar.DoenerOrder.Core.Data.DAO;
using Isitar.DoenerOrder.Core.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Isitar.DoenerOrder.Core.Data
{
    public class DoenerOrderContext : DbContext
    {
        public DoenerOrderContext(DbContextOptions<DoenerOrderContext> options) : base(options)
        {
        }

        internal virtual DbSet<BulkOrder> BulkOrders { get; set; }
        internal virtual DbSet<Order> Orders { get; set; }
        internal virtual DbSet<OrderLine> OrderLines { get; set; }

        internal virtual DbSet<Supplier> Suppliers { get; set; }
        internal virtual DbSet<Product> Products { get; set; }
        internal virtual DbSet<ProductIngredient> ProductIngredients { get; set; }
        internal virtual DbSet<Ingredient> Ingredients { get; set; }

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