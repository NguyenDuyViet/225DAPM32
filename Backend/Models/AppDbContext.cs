using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodImage> FoodImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartFood> CartFoods { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<OrderPromotion> OrderPromotions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderRestaurant> OrderRestaurants { get; set; }
        public DbSet<OrderFood> OrderFoods { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewFood> ReviewFoods { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table names to match DB.sql
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Notification>().ToTable("Notification");
            modelBuilder.Entity<Voucher>().ToTable("Voucher");
            modelBuilder.Entity<Restaurant>().ToTable("Restaurant");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Food>().ToTable("Food");
            modelBuilder.Entity<FoodImage>().ToTable("Food_Image");
            modelBuilder.Entity<Cart>().ToTable("Cart");
            modelBuilder.Entity<CartFood>().ToTable("Cart_Food");
            modelBuilder.Entity<Promotion>().ToTable("Promotion");
            modelBuilder.Entity<OrderPromotion>().ToTable("Order_Promotion");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderRestaurant>().ToTable("Order_Restaurant");
            modelBuilder.Entity<OrderFood>().ToTable("Order_Food");
            modelBuilder.Entity<Review>().ToTable("Review");
            modelBuilder.Entity<ReviewFood>().ToTable("Review_Food");
            modelBuilder.Entity<Driver>().ToTable("Driver");
            modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethod");
            modelBuilder.Entity<Complaint>().ToTable("Complaint");
            modelBuilder.Entity<SystemLog>().ToTable("SystemLog");

            // Configure primary keys
            modelBuilder.Entity<User>().HasKey(u => u.IdUser);
            modelBuilder.Entity<Role>().HasKey(r => r.IdRole);
            modelBuilder.Entity<Address>().HasKey(a => a.IdAddress);
            modelBuilder.Entity<Notification>().HasKey(n => n.IdNoti);
            modelBuilder.Entity<Voucher>().HasKey(v => v.IdVoucher);
            modelBuilder.Entity<Restaurant>().HasKey(r => r.IdRestaurant);
            modelBuilder.Entity<Category>().HasKey(c => c.IdCategory);
            modelBuilder.Entity<Food>().HasKey(f => f.IdFood);
            modelBuilder.Entity<FoodImage>().HasKey(fi => fi.IdFoodimage);
            modelBuilder.Entity<Cart>().HasKey(c => c.IdCart);
            modelBuilder.Entity<CartFood>().HasKey(cf => cf.IdCartFood);
            modelBuilder.Entity<Promotion>().HasKey(p => p.IdPromo);
            modelBuilder.Entity<OrderPromotion>().HasKey(op => new { op.IdOrder, op.IdPromo });
            modelBuilder.Entity<Order>().HasKey(o => o.IdOrder);
            modelBuilder.Entity<OrderRestaurant>().HasKey(or => or.IdOrder);
            modelBuilder.Entity<OrderFood>().HasKey(of => of.IdOrderFood);
            modelBuilder.Entity<Review>().HasKey(r => r.IdReview);
            modelBuilder.Entity<ReviewFood>().HasKey(rf => rf.IdReviewFood);
            modelBuilder.Entity<Driver>().HasKey(d => d.IdDriver);
            modelBuilder.Entity<PaymentMethod>().HasKey(pm => pm.IdTransaction);
            modelBuilder.Entity<Complaint>().HasKey(c => c.IdComplaint);
            modelBuilder.Entity<SystemLog>().HasKey(sl => sl.IdLog);

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.IdRole);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.IdUser);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.IdUser);

            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.User)
                .WithMany(u => u.Vouchers)
                .HasForeignKey(v => v.IdUser);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.IdUser);

            modelBuilder.Entity<CartFood>()
                .HasOne(cf => cf.Cart)
                .WithMany(c => c.CartFoods)
                .HasForeignKey(cf => cf.IdCart);

            modelBuilder.Entity<CartFood>()
                .HasOne(cf => cf.Food)
                .WithMany(f => f.CartFoods)
                .HasForeignKey(cf => cf.IdFood);

            modelBuilder.Entity<Food>()
                .HasOne(f => f.Category)
                .WithMany(c => c.Foods)
                .HasForeignKey(f => f.IdCategory);

            modelBuilder.Entity<Food>()
                .HasOne(f => f.Restaurant)
                .WithMany(r => r.Foods)
                .HasForeignKey(f => f.IdRestaurant);

            modelBuilder.Entity<FoodImage>()
                .HasOne(fi => fi.Food)
                .WithMany(f => f.FoodImages)
                .HasForeignKey(fi => fi.IdFood);

            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.Restaurant)
                .WithMany(r => r.Promotions)
                .HasForeignKey(p => p.IdRestaurant);

            modelBuilder.Entity<OrderPromotion>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderPromotions)
                .HasForeignKey(op => op.IdOrder);

            modelBuilder.Entity<OrderPromotion>()
                .HasOne(op => op.Promotion)
                .WithMany(p => p.OrderPromotions)
                .HasForeignKey(op => op.IdPromo);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.IdUser);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.IdAddress);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Driver)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.IdDriver);

            modelBuilder.Entity<OrderRestaurant>()
                .HasOne(or => or.Order)
                .WithOne(o => o.OrderRestaurant)
                .HasForeignKey<OrderRestaurant>(or => or.IdOrder);

            modelBuilder.Entity<OrderRestaurant>()
                .HasOne(or => or.Restaurant)
                .WithMany(r => r.OrderRestaurants)
                .HasForeignKey(or => or.IdRestaurant);

            modelBuilder.Entity<OrderFood>()
                .HasOne(of => of.Order)
                .WithMany(o => o.OrderFoods)
                .HasForeignKey(of => of.IdOrder);

            modelBuilder.Entity<OrderFood>()
                .HasOne(of => of.Food)
                .WithMany(f => f.OrderFoods)
                .HasForeignKey(of => of.IdFood);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.IdUser);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Order)
                .WithOne(o => o.Review)
                .HasForeignKey<Review>(r => r.IdOrder);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Restaurant)
                .WithMany(res => res.Reviews)
                .HasForeignKey(r => r.IdRestaurant);

            modelBuilder.Entity<ReviewFood>()
                .HasOne(rf => rf.Review)
                .WithMany(r => r.ReviewFoods)
                .HasForeignKey(rf => rf.IdReview);

            modelBuilder.Entity<ReviewFood>()
                .HasOne(rf => rf.Food)
                .WithMany(f => f.ReviewFoods)
                .HasForeignKey(rf => rf.IdFood);

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.User)
                .WithOne(u => u.Driver)
                .HasForeignKey<Driver>(d => d.IdUser);

            modelBuilder.Entity<PaymentMethod>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.PaymentMethods)
                .HasForeignKey(pm => pm.IdUser);

            modelBuilder.Entity<PaymentMethod>()
                .HasOne(pm => pm.Order)
                .WithOne(o => o.PaymentMethodEntity)
                .HasForeignKey<PaymentMethod>(pm => pm.IdOrder);

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Order)
                .WithOne(o => o.Complaint)
                .HasForeignKey<Complaint>(c => c.IdOrder);

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.User)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.IdUser);

            modelBuilder.Entity<SystemLog>()
                .HasOne(sl => sl.User)
                .WithMany(u => u.SystemLogs)
                .HasForeignKey(sl => sl.IdUser);
        }
    }
}