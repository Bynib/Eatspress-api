using Microsoft.EntityFrameworkCore;

namespace Eatspress.Models
{
    public class EatspressContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }

        public EatspressContext(DbContextOptions<EatspressContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key for CartDetails
            modelBuilder.Entity<CartDetails>()
                .HasKey(cd => new { cd.Order_Id, cd.Item_Id });

            // User -> Address
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithMany(a => a.Users)
                .HasForeignKey(u => u.Address_Id);

            // User -> Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.Role_Id);

            // User -> Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.Customer_Id);

            // Order -> OrderStatus
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.Status_Id);

            // FoodItem -> FoodCategory
            modelBuilder.Entity<FoodItem>()
                .HasOne(fi => fi.Category)
                .WithMany(fc => fc.FoodItems)
                .HasForeignKey(fi => fi.Category_Id);

            // CartDetails -> Order
            modelBuilder.Entity<CartDetails>()
                .HasOne(cd => cd.Order)
                .WithMany(o => o.CartDetails)
                .HasForeignKey(cd => cd.Order_Id);

            // CartDetails -> FoodItem
            modelBuilder.Entity<CartDetails>()
                .HasOne(cd => cd.Item)
                .WithMany(fi => fi.CartDetails)
                .HasForeignKey(cd => cd.Item_Id);
        }
    }
}
