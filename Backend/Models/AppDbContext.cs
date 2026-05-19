using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r => r.IdRole);
                entity.Property(r => r.IdRole).HasColumnName("id_Role").ValueGeneratedNever();
                entity.Property(r => r.RoleName).HasColumnName("role_Name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.IdUser);
                entity.Property(u => u.IdUser).HasColumnName("id_User").ValueGeneratedNever();
                entity.Property(u => u.IdRole).HasColumnName("id_Role");
                entity.Property(u => u.Username).HasColumnName("username");
                entity.Property(u => u.Password).HasColumnName("password");
                entity.Property(u => u.FullName).HasColumnName("fullName");
                entity.Property(u => u.Phone).HasColumnName("phone");
                entity.Property(u => u.Email).HasColumnName("email");
                entity.Property(u => u.Address).HasColumnName("address");
                entity.Property(u => u.Avatar).HasColumnName("avatar");
                entity.Property(u => u.Status).HasColumnName("status");
                entity.Property(u => u.CreatedAt).HasColumnName("created_At");
                entity.Property(u => u.LastOnline).HasColumnName("lastOnline");
                entity.Property(u => u.CurrentLat).HasColumnName("current_Lat").HasPrecision(10, 7);
                entity.Property(u => u.CurrentLng).HasColumnName("current_Lng").HasPrecision(10, 7);
                entity.Property(u => u.CancelRate).HasColumnName("cancel_Rate");
                entity.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.IdRole);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");
                entity.HasKey(n => n.IdNoti);
                entity.Property(n => n.IdNoti).HasColumnName("id_Noti").ValueGeneratedNever();
                entity.Property(n => n.IdUser).HasColumnName("id_User");
                entity.Property(n => n.Title).HasColumnName("title");
                entity.Property(n => n.Body).HasColumnName("body");
                entity.Property(n => n.OrderId).HasColumnName("orderId");
                entity.Property(n => n.IsRead).HasColumnName("is_Read");
                entity.Property(n => n.CreatedAt).HasColumnName("created_At");
                entity.HasOne(n => n.User).WithMany(u => u.Notifications).HasForeignKey(n => n.IdUser);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Voucher");
                entity.HasKey(v => v.IdVoucher);
                entity.Property(v => v.IdVoucher).HasColumnName("id_Voucher").ValueGeneratedNever();
                entity.Property(v => v.Code).HasColumnName("code");
                entity.Property(v => v.IdUser).HasColumnName("id_User");
                entity.Property(v => v.Value).HasColumnName("value").HasPrecision(10, 2);
                entity.Property(v => v.Expiry).HasColumnName("expiry");
                entity.Property(v => v.Used).HasColumnName("used");
                entity.HasOne(v => v.User).WithMany(u => u.Vouchers).HasForeignKey(v => v.IdUser);
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("Restaurant");
                entity.HasKey(r => r.IdRestaurant);
                entity.Property(r => r.IdRestaurant).HasColumnName("id_Restaurant").ValueGeneratedNever();
                entity.Property(r => r.NameRestaurant).HasColumnName("name_Restaurant");
                entity.Property(r => r.Description).HasColumnName("description");
                entity.Property(r => r.Image).HasColumnName("image");
                entity.Property(r => r.Address).HasColumnName("address");
                entity.Property(r => r.OpenTime).HasColumnName("openTime");
                entity.Property(r => r.CloseTime).HasColumnName("closeTime");
                entity.Property(r => r.Lat).HasColumnName("lat").HasPrecision(10, 7);
                entity.Property(r => r.Lng).HasColumnName("lng").HasPrecision(10, 7);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(c => c.IdCategory);
                entity.Property(c => c.IdCategory).HasColumnName("id_Category").ValueGeneratedNever();
                entity.Property(c => c.Name).HasColumnName("name");
                entity.Property(c => c.Icon).HasColumnName("icon");
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("Food");
                entity.HasKey(f => f.IdFood);
                entity.Property(f => f.IdFood).HasColumnName("id_Food").ValueGeneratedNever();
                entity.Property(f => f.IdCategory).HasColumnName("id_Category");
                entity.Property(f => f.IdRestaurant).HasColumnName("id_Restaurant");
                entity.Property(f => f.Name).HasColumnName("name");
                entity.Property(f => f.Description).HasColumnName("description");
                entity.Property(f => f.Image).HasColumnName("image");
                entity.Property(f => f.Video).HasColumnName("video");
                entity.Property(f => f.Price).HasColumnName("price").HasPrecision(10, 2);
                entity.Property(f => f.Discount).HasColumnName("discount").HasPrecision(10, 2);
                entity.Property(f => f.CookCount).HasColumnName("cook_Count");
                entity.Property(f => f.PrepTime).HasColumnName("prep_Time");
                entity.HasOne(f => f.Category).WithMany(c => c.Foods).HasForeignKey(f => f.IdCategory);
                entity.HasOne(f => f.Restaurant).WithMany(r => r.Foods).HasForeignKey(f => f.IdRestaurant);
            });

            modelBuilder.Entity<FoodImage>(entity =>
            {
                entity.ToTable("Food_Image");
                entity.HasKey(fi => fi.IdFoodimage);
                entity.Property(fi => fi.IdFoodimage).HasColumnName("id_Foodimage").ValueGeneratedNever();
                entity.Property(fi => fi.IdFood).HasColumnName("id_Food");
                entity.Property(fi => fi.Image).HasColumnName("image");
                entity.HasOne(fi => fi.Food).WithMany(f => f.FoodImages).HasForeignKey(fi => fi.IdFood);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");
                entity.HasKey(c => c.IdCart);
                entity.Property(c => c.IdCart).HasColumnName("id_Cart").ValueGeneratedNever();
                entity.Property(c => c.IdUser).HasColumnName("id_User");
                entity.Property(c => c.Total).HasColumnName("total");
                entity.Property(c => c.CreatedAt).HasColumnName("created_At");
                entity.Property(c => c.UpdateAt).HasColumnName("update_At");
                entity.HasOne(c => c.User).WithMany(u => u.Carts).HasForeignKey(c => c.IdUser);
            });

            modelBuilder.Entity<CartFood>(entity =>
            {
                entity.ToTable("Cart_Food");
                entity.HasKey(cf => cf.IdCartFood);
                entity.Property(cf => cf.IdCartFood).HasColumnName("id_CartFood").ValueGeneratedNever();
                entity.Property(cf => cf.IdCart).HasColumnName("id_Cart");
                entity.Property(cf => cf.IdFood).HasColumnName("id_Food");
                entity.Property(cf => cf.Quantity).HasColumnName("quantity");
                entity.Property(cf => cf.Note).HasColumnName("note");
                entity.HasOne(cf => cf.Cart).WithMany(c => c.CartFoods).HasForeignKey(cf => cf.IdCart);
                entity.HasOne(cf => cf.Food).WithMany(f => f.CartFoods).HasForeignKey(cf => cf.IdFood);
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.ToTable("Promotion");
                entity.HasKey(p => p.IdPromo);
                entity.Property(p => p.IdPromo).HasColumnName("id_Promo").ValueGeneratedNever();
                entity.Property(p => p.Type).HasColumnName("type");
                entity.Property(p => p.Value).HasColumnName("value").HasPrecision(10, 2);
                entity.Property(p => p.MinOrderValue).HasColumnName("min_OrderValue").HasPrecision(10, 2);
                entity.Property(p => p.MaxDiscount).HasColumnName("max_Discount").HasPrecision(10, 2);
                entity.Property(p => p.UsageLimit).HasColumnName("usage_Limit");
                entity.Property(p => p.UsedCount).HasColumnName("used_Count");
                entity.Property(p => p.StartDate).HasColumnName("start_Date");
                entity.Property(p => p.EndDate).HasColumnName("end_Date");
                entity.Property(p => p.IdRestaurant).HasColumnName("id_Restaurant");
                entity.HasOne(p => p.Restaurant).WithMany(r => r.Promotions).HasForeignKey(p => p.IdRestaurant);
            });

            modelBuilder.Entity<OrderPromotion>(entity =>
            {
                entity.ToTable("Order_Promotion");
                entity.HasKey(op => new { op.IdOrder, op.IdPromo });
                entity.Property(op => op.IdOrder).HasColumnName("id_Order");
                entity.Property(op => op.IdPromo).HasColumnName("id_Promo");
                entity.Property(op => op.DiscountAmount).HasColumnName("discount_Amount").HasPrecision(10, 2);
                entity.HasOne(op => op.Order).WithMany(o => o.OrderPromotions).HasForeignKey(op => op.IdOrder);
                entity.HasOne(op => op.Promotion).WithMany(p => p.OrderPromotions).HasForeignKey(op => op.IdPromo);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.IdOrder);
                entity.Property(o => o.IdOrder).HasColumnName("id_Order").ValueGeneratedNever();
                entity.Property(o => o.IdUser).HasColumnName("id_User");
                entity.Property(o => o.IdRestaurant).HasColumnName("id_Restaurant");
                entity.Property(o => o.IdDriver).HasColumnName("id_Driver");
                entity.Property(o => o.IdVoucher).HasColumnName("id_Voucher");
                entity.Property(o => o.OrderCode).HasColumnName("order_Code");
                entity.Property(o => o.DeliveryAddress).HasColumnName("delivery_Address");
                entity.Property(o => o.DeliveryLat).HasColumnName("delivery_Lat").HasPrecision(10, 7);
                entity.Property(o => o.DeliveryLng).HasColumnName("delivery_Lng").HasPrecision(10, 7);
                entity.Property(o => o.PaymentMethod).HasColumnName("paymentMethod");
                entity.Property(o => o.FoodAmount).HasColumnName("food_Amount").HasPrecision(10, 2);
                entity.Property(o => o.ShippingFee).HasColumnName("shippingFee").HasPrecision(10, 2);
                entity.Property(o => o.Discount).HasColumnName("discount").HasPrecision(10, 2);
                entity.Property(o => o.FinalTotal).HasColumnName("finalTotal").HasPrecision(10, 2);
                entity.Property(o => o.PaymentStatus).HasColumnName("paymentStatus");
                entity.Property(o => o.Status).HasColumnName("status");
                entity.Property(o => o.Note).HasColumnName("note");
                entity.Property(o => o.CancelReason).HasColumnName("cancel_Reason");
                entity.Property(o => o.CreatedAt).HasColumnName("created_At");
                entity.Property(o => o.UpdatedAt).HasColumnName("updated_At");
                entity.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.IdUser);
                entity.HasOne(o => o.Restaurant).WithMany(r => r.Orders).HasForeignKey(o => o.IdRestaurant);
                entity.HasOne(o => o.Driver).WithMany(d => d.Orders).HasForeignKey(o => o.IdDriver);
                entity.HasOne(o => o.Voucher).WithMany().HasForeignKey(o => o.IdVoucher);
            });

            modelBuilder.Entity<OrderFood>(entity =>
            {
                entity.ToTable("Order_Food");
                entity.HasKey(of => of.IdOrderFood);
                entity.Property(of => of.IdOrderFood).HasColumnName("id_OrderFood").ValueGeneratedNever();
                entity.Property(of => of.IdOrder).HasColumnName("id_Order");
                entity.Property(of => of.IdFood).HasColumnName("id_Food");
                entity.Property(of => of.Quantity).HasColumnName("quantity");
                entity.Property(of => of.UnitPrice).HasColumnName("unit_Price").HasPrecision(10, 2);
                entity.Property(of => of.TotalPrice).HasColumnName("total_Price").HasPrecision(10, 2);
                entity.Property(of => of.Note).HasColumnName("note");
                entity.HasOne(of => of.Order).WithMany(o => o.OrderFoods).HasForeignKey(of => of.IdOrder);
                entity.HasOne(of => of.Food).WithMany(f => f.OrderFoods).HasForeignKey(of => of.IdFood);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");
                entity.HasKey(r => r.IdReview);
                entity.Property(r => r.IdReview).HasColumnName("id_Review").ValueGeneratedNever();
                entity.Property(r => r.IdUser).HasColumnName("id_User");
                entity.Property(r => r.IdOrder).HasColumnName("id_Order");
                entity.Property(r => r.IdRestaurant).HasColumnName("id_Restaurant");
                entity.Property(r => r.FoodRating).HasColumnName("food_rating");
                entity.Property(r => r.DriverRating).HasColumnName("driver_rating");
                entity.Property(r => r.CommentForRes).HasColumnName("comment_ForRes");
                entity.Property(r => r.CommentForShipper).HasColumnName("comment_ForShipper");
                entity.Property(r => r.CreatedAt).HasColumnName("created_At");
                entity.HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.IdUser);
                entity.HasOne(r => r.Order).WithOne(o => o.Review).HasForeignKey<Review>(r => r.IdOrder);
                entity.HasOne(r => r.Restaurant).WithMany(res => res.Reviews).HasForeignKey(r => r.IdRestaurant);
            });

            modelBuilder.Entity<ReviewFood>(entity =>
            {
                entity.ToTable("Review_Food");
                entity.HasKey(rf => rf.IdReviewFood);
                entity.Property(rf => rf.IdReviewFood).HasColumnName("id_ReviewFood").ValueGeneratedNever();
                entity.Property(rf => rf.IdReview).HasColumnName("id_Review");
                entity.Property(rf => rf.IdFood).HasColumnName("id_Food");
                entity.Property(rf => rf.Rating).HasColumnName("rating");
                entity.Property(rf => rf.Comment).HasColumnName("comment");
                entity.Property(rf => rf.Image).HasColumnName("image");
                entity.Property(rf => rf.Video).HasColumnName("video");
                entity.HasOne(rf => rf.Review).WithMany(r => r.ReviewFoods).HasForeignKey(rf => rf.IdReview);
                entity.HasOne(rf => rf.Food).WithMany(f => f.ReviewFoods).HasForeignKey(rf => rf.IdFood);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("Driver");
                entity.HasKey(d => d.IdDriver);
                entity.Property(d => d.IdDriver).HasColumnName("id_Driver").ValueGeneratedNever();
                entity.Property(d => d.IdUser).HasColumnName("id_User");
                entity.Property(d => d.LicensePlate).HasColumnName("license_plate");
                entity.Property(d => d.Address).HasColumnName("address");
                entity.Property(d => d.ExpRank).HasColumnName("expRank");
                entity.Property(d => d.DescStatus).HasColumnName("desc_Status");
                entity.Property(d => d.CurrentLat).HasColumnName("current_Lat").HasPrecision(10, 7);
                entity.Property(d => d.CurrentLng).HasColumnName("current_Lng").HasPrecision(10, 7);
                entity.Property(d => d.IsBusy).HasColumnName("is_Busy");
                entity.Property(d => d.RateAvg).HasColumnName("rate_Avg").HasPrecision(2, 1);
                entity.Property(d => d.TotalOrders).HasColumnName("total_Orders");
                entity.HasOne(d => d.User).WithOne(u => u.Driver).HasForeignKey<Driver>(d => d.IdUser);
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");
                entity.HasKey(pm => pm.IdTransaction);
                entity.Property(pm => pm.IdTransaction).HasColumnName("id_Transaction").ValueGeneratedNever();
                entity.Property(pm => pm.IdUser).HasColumnName("id_User");
                entity.Property(pm => pm.IdOrder).HasColumnName("id_Order");
                entity.Property(pm => pm.Method).HasColumnName("method");
                entity.Property(pm => pm.Amount).HasColumnName("amount").HasPrecision(10, 2);
                entity.HasOne(pm => pm.User).WithMany(u => u.PaymentMethods).HasForeignKey(pm => pm.IdUser);
                entity.HasOne(pm => pm.Order).WithOne(o => o.PaymentMethodEntity).HasForeignKey<PaymentMethod>(pm => pm.IdOrder);
            });

            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.ToTable("Complaint");
                entity.HasKey(c => c.IdComplaint);
                entity.Property(c => c.IdComplaint).HasColumnName("id_Complaint").ValueGeneratedNever();
                entity.Property(c => c.IdOrder).HasColumnName("id_Order");
                entity.Property(c => c.IdUser).HasColumnName("id_User");
                entity.Property(c => c.Type).HasColumnName("type");
                entity.Property(c => c.Description).HasColumnName("description");
                entity.Property(c => c.Image).HasColumnName("image");
                entity.Property(c => c.Status).HasColumnName("status");
                entity.Property(c => c.HandledBy).HasColumnName("handled_By");
                entity.Property(c => c.ReceivedAt).HasColumnName("received_At");
                entity.Property(c => c.ResolvedAt).HasColumnName("resolved_At");
                entity.HasOne(c => c.Order).WithOne(o => o.Complaint).HasForeignKey<Complaint>(c => c.IdOrder);
                entity.HasOne(c => c.User).WithMany(u => u.Complaints).HasForeignKey(c => c.IdUser);
            });

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.ToTable("SystemLog");
                entity.HasKey(sl => sl.IdLog);
                entity.Property(sl => sl.IdLog).HasColumnName("id_Log").ValueGeneratedNever();
                entity.Property(sl => sl.IdUser).HasColumnName("id_User");
                entity.Property(sl => sl.Action).HasColumnName("action");
                entity.Property(sl => sl.Entity).HasColumnName("entity");
                entity.Property(sl => sl.EntityId).HasColumnName("entity_Id");
                entity.Property(sl => sl.OldValue).HasColumnName("old_Value");
                entity.Property(sl => sl.NewValue).HasColumnName("new_Value");
                entity.Property(sl => sl.IpAddress).HasColumnName("ip_Address");
                entity.Property(sl => sl.CreatedAt).HasColumnName("created_At");
                entity.HasOne(sl => sl.User).WithMany(u => u.SystemLogs).HasForeignKey(sl => sl.IdUser);
            });
        }
    }
}
