using Eatspress.Models;
using Microsoft.EntityFrameworkCore;

namespace Eatspress.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<FoodCategory> FoodCategories => Set<FoodCategory>();
        public DbSet<FoodItem> FoodItems => Set<FoodItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetails> OrderDetails => Set<OrderDetails>();
        public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //address
            modelBuilder.Entity<Address>()
                .HasOne(u => u.User)
                .WithMany(a => a.Addresses)
                .HasForeignKey(u => u.Address_Id);

            //cart
            modelBuilder.Entity<Cart>()
                .HasKey(c => new { c.User_id, c.Item_Id });
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.User_id);
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.FoodItem)
                .WithMany(fi => fi.Carts)
                .HasForeignKey(c => c.Item_Id);

            //fooditem
            modelBuilder.Entity<FoodItem>()
                .HasOne(fc => fc.Category)
                .WithMany(fi => fi.FoodItems)
                .HasForeignKey(fc => fc.Category_Id);

            //order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(os => os.User_Id);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.Address_Id);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany(os => os.Orders)
                .HasForeignKey(os => os.Status_Id);

            //oderdetails
            modelBuilder.Entity<OrderDetails>()
                .HasKey(od => new { od.Order_Id, od.Item_Id });
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.Order_Id);
            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.FoodItem)
                .WithMany(fi => fi.OrderDetails)
                .HasForeignKey(od => od.Item_Id);

            //user
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.Role_Id);

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Role_Id = 1, Role_Title = "Admin" },
                new UserRole { Role_Id = 2, Role_Title = "User" }
            );

            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Status_Id = 1, Status_Type = "Pending" },
                new OrderStatus { Status_Id = 2, Status_Type = "Preparing" },
                new OrderStatus { Status_Id = 3, Status_Type = "Ready" },
                new OrderStatus { Status_Id = 4, Status_Type = "Delivered" },
                new OrderStatus { Status_Id = 5, Status_Type = "Cancelled" }
            );

            modelBuilder.Entity<FoodCategory>().HasData(
                new FoodCategory { Category_Id = 1, Category_Type = "Appetizer" },
                new FoodCategory { Category_Id = 2, Category_Type = "Main Course" },
                new FoodCategory { Category_Id = 3, Category_Type = "Dessert" },
                new FoodCategory { Category_Id = 4, Category_Type = "Beverage" }
            );
        }
    }
}
