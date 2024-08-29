using Agile_Ecommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agile_Ecommerce.Repository
{
	public class DataContext : IdentityDbContext<AppUserModel>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<WishListItems> WishListItems { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Khai báo mối quan hệ giữa OrderModel và OrderDetails
            modelBuilder.Entity<OrderModel>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderModelId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
