using Backend.Models;

namespace Backend.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users.Any()) return;

            var now = DateTime.Now;

            var roles = new Role[]
            {
                new Role { IdRole = 1, RoleName = "admin" },
                new Role { IdRole = 2, RoleName = "customer" },
                new Role { IdRole = 3, RoleName = "restaurant" },
                new Role { IdRole = 4, RoleName = "shipper" }
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();

            var users = new User[]
            {
                new User { IdUser = 1, IdRole = 1, Username = "admin", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Quản trị viên", Phone = "0900000001", Email = "admin@example.com", Address = "Quận 1, TP.HCM", Avatar = "images/users/admin.jpg", Status = "active", CreatedAt = now, LastOnline = now, CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0 },
                new User { IdUser = 2, IdRole = 2, Username = "customer01", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Nguyễn Văn An", Phone = "0901000001", Email = "customer01@example.com", Address = "123 Nguyễn Trãi, Quận 1, TP.HCM", Avatar = "images/users/customer01.jpg", Status = "active", CreatedAt = now, LastOnline = now, CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.02f },
                new User { IdUser = 3, IdRole = 2, Username = "customer02", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Trần Thị Bình", Phone = "0901000002", Email = "customer02@example.com", Address = "45 Lê Lợi, Quận 1, TP.HCM", Avatar = "images/users/customer02.jpg", Status = "active", CreatedAt = now, LastOnline = now.AddMinutes(-15), CurrentLat = 10.776889m, CurrentLng = 106.700806m, CancelRate = 0.01f },
                new User { IdUser = 4, IdRole = 2, Username = "customer03", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Lê Minh Châu", Phone = "0901000003", Email = "customer03@example.com", Address = "78 Phan Xích Long, Phú Nhuận, TP.HCM", Avatar = "images/users/customer03.jpg", Status = "active", CreatedAt = now, LastOnline = now.AddHours(-1), CurrentLat = 10.801337m, CurrentLng = 106.714021m, CancelRate = 0.04f },
                new User { IdUser = 5, IdRole = 3, Username = "restaurant01", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Chủ Bếp Nhà Việt", Phone = "0903000001", Email = "restaurant01@example.com", Address = "20 Nguyễn Thị Minh Khai, Quận 1", Avatar = "images/users/restaurant01.jpg", Status = "active", CreatedAt = now, LastOnline = now, CurrentLat = 10.782500m, CurrentLng = 106.700000m, CancelRate = 0 },
                new User { IdUser = 6, IdRole = 4, Username = "shipper01", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Phạm Quốc Duy", Phone = "0902000001", Email = "shipper01@example.com", Address = "Quận 1, TP.HCM", Avatar = "images/users/shipper01.jpg", Status = "active", CreatedAt = now, LastOnline = now, CurrentLat = 10.762000m, CurrentLng = 106.660000m, CancelRate = 0 },
                new User { IdUser = 7, IdRole = 4, Username = "shipper02", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Võ Hoàng Nam", Phone = "0902000002", Email = "shipper02@example.com", Address = "Phú Nhuận, TP.HCM", Avatar = "images/users/shipper02.jpg", Status = "active", CreatedAt = now, LastOnline = now.AddMinutes(-5), CurrentLat = 10.801337m, CurrentLng = 106.714021m, CancelRate = 0 }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var categories = new Category[]
            {
                new Category { IdCategory = 1, Name = "Đồ ăn", Icon = "icons/do-an.png" },
                new Category { IdCategory = 2, Name = "Đồ uống", Icon = "icons/do-uong.png" },
                new Category { IdCategory = 3, Name = "Đồ chay", Icon = "icons/do-chay.png" },
                new Category { IdCategory = 4, Name = "Bánh kem", Icon = "icons/banh-kem.png" },
                new Category { IdCategory = 5, Name = "Tráng miệng", Icon = "icons/trang-mieng.png" },
                new Category { IdCategory = 6, Name = "Món lẩu", Icon = "icons/mon-lau.png" },
                new Category { IdCategory = 7, Name = "Hải sản", Icon = "icons/hai-san.png" },
                new Category { IdCategory = 8, Name = "Cơm hộp", Icon = "icons/com-hop.png" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var restaurants = new Restaurant[]
            {
                new Restaurant { IdRestaurant = 1, NameRestaurant = "Bếp Nhà Việt", Description = "Món cơm nhà và món mặn Việt Nam", Image = "images/restaurants/bep-nha-viet.jpg", Address = "20 Nguyễn Thị Minh Khai, Quận 1", OpenTime = new TimeSpan(7, 0, 0), CloseTime = new TimeSpan(21, 30, 0), Lat = 10.782500m, Lng = 106.700000m },
                new Restaurant { IdRestaurant = 2, NameRestaurant = "Gà Ngon 3 Miền", Description = "Các món gà kho, nướng, rang muối", Image = "images/restaurants/ga-ngon-3-mien.jpg", Address = "88 Cách Mạng Tháng 8, Quận 3", OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Lat = 10.777300m, Lng = 106.686600m },
                new Restaurant { IdRestaurant = 3, NameRestaurant = "Lẩu Gà Sài Gòn", Description = "Lẩu gà thuốc bắc và lẩu gà ớt hiểm", Image = "images/restaurants/lau-ga-sai-gon.jpg", Address = "15 Phan Xích Long, Phú Nhuận", OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(23, 0, 0), Lat = 10.801000m, Lng = 106.710000m },
                new Restaurant { IdRestaurant = 4, NameRestaurant = "Quán Hải Sản Nhà Làm", Description = "Mực, tôm và hải sản chế biến nóng", Image = "images/restaurants/hai-san-nha-lam.jpg", Address = "51 Trần Não, TP Thủ Đức", OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(22, 30, 0), Lat = 10.787000m, Lng = 106.749000m }
            };
            context.Restaurants.AddRange(restaurants);
            context.SaveChanges();

            var foods = new Food[]
            {
                new Food { IdFood = 1, IdCategory = 1, IdRestaurant = 1, Name = "Bánh xèo", Description = "Bánh xèo vàng giòn ăn kèm rau sống và nước mắm chua ngọt", Image = "images/foods/Bánh xèo.jpg", Price = 45000m, Discount = 5000m, CookCount = 86, PrepTime = 15 },
                new Food { IdFood = 2, IdCategory = 8, IdRestaurant = 1, Name = "Cá kho tộ", Description = "Cá kho tộ đậm vị, dùng với cơm trắng nóng", Image = "images/foods/Cá kho tộ.jpg", Price = 55000m, Discount = 5000m, CookCount = 120, PrepTime = 18 },
                new Food { IdFood = 3, IdCategory = 8, IdRestaurant = 1, Name = "Bò xào ớt chuông", Description = "Thịt bò mềm xào ớt chuông, hành tây và sốt đậm đà", Image = "images/foods/Bò xào ớt chuông.jpg", Price = 65000m, Discount = 7000m, CookCount = 72, PrepTime = 12 },
                new Food { IdFood = 4, IdCategory = 1, IdRestaurant = 2, Name = "Cánh gà chiên nước mắm", Description = "Cánh gà chiên giòn phủ sốt nước mắm tỏi", Image = "images/foods/Cánh gà chiên nước mắm.jpg", Price = 59000m, Discount = 6000m, CookCount = 140, PrepTime = 15 },
                new Food { IdFood = 5, IdCategory = 1, IdRestaurant = 2, Name = "Gà rang muối", Description = "Gà rang muối giòn thơm, ăn kèm rau răm", Image = "images/foods/Gà rang muối.jpg", Price = 72000m, Discount = 8000m, CookCount = 96, PrepTime = 18 },
                new Food { IdFood = 6, IdCategory = 1, IdRestaurant = 2, Name = "Gà nướng muối ớt", Description = "Gà nướng cay nhẹ, da vàng giòn", Image = "images/foods/Gà nướng muối ớt.jpg", Price = 89000m, Discount = 10000m, CookCount = 110, PrepTime = 25 },
                new Food { IdFood = 7, IdCategory = 8, IdRestaurant = 2, Name = "Gà kho nấm", Description = "Gà kho nấm mềm thơm, hợp dùng với cơm hộp", Image = "images/foods/Gà kho nấm.jpg", Price = 62000m, Discount = 5000m, CookCount = 75, PrepTime = 18 },
                new Food { IdFood = 8, IdCategory = 6, IdRestaurant = 3, Name = "Lẩu gà thuốc bắc", Description = "Lẩu gà thuốc bắc bổ dưỡng với nước dùng ngọt thanh", Image = "images/foods/Lẩu gà thuốc bắc.jpg", Price = 189000m, Discount = 15000m, CookCount = 52, PrepTime = 25 },
                new Food { IdFood = 9, IdCategory = 6, IdRestaurant = 3, Name = "Lẩu gà tiềm ớt hiểm", Description = "Lẩu gà tiềm ớt hiểm cay ấm, thơm đậm", Image = "images/foods/Lẩu gà tiềm ớt hiểm.jpg", Price = 199000m, Discount = 20000m, CookCount = 60, PrepTime = 25 },
                new Food { IdFood = 10, IdCategory = 7, IdRestaurant = 4, Name = "Mực chiên nước mắm", Description = "Mực chiên nước mắm tỏi, giòn dai vừa miệng", Image = "images/foods/Mực chiên nước mắm.jpg", Price = 89000m, Discount = 8000m, CookCount = 58, PrepTime = 16 },
                new Food { IdFood = 11, IdCategory = 7, IdRestaurant = 4, Name = "Tôm khìa nước dừa", Description = "Tôm khìa nước dừa béo thơm, vị ngọt tự nhiên", Image = "images/foods/Tôm khìa nước dừa.jpg", Price = 95000m, Discount = 9000m, CookCount = 64, PrepTime = 18 },
                new Food { IdFood = 12, IdCategory = 7, IdRestaurant = 4, Name = "Tôm xào chua ngọt", Description = "Tôm xào chua ngọt cùng rau củ bắt vị", Image = "images/foods/tôm xào chua ngọt.jpg", Price = 92000m, Discount = 7000m, CookCount = 61, PrepTime = 16 }
            };
            context.Foods.AddRange(foods);
            context.SaveChanges();

            var vouchers = new Voucher[]
            {
                new Voucher { IdVoucher = 1, Code = "WELCOME10", IdUser = 2, Value = 10000m, Expiry = now.AddDays(30), Used = false },
                new Voucher { IdVoucher = 2, Code = "LAUGA20", IdUser = 3, Value = 20000m, Expiry = now.AddDays(14), Used = false },
                new Voucher { IdVoucher = 3, Code = "COMTRUA15", IdUser = 4, Value = 15000m, Expiry = now.AddDays(20), Used = false }
            };
            context.Vouchers.AddRange(vouchers);
            context.SaveChanges();

            var carts = new Cart[]
            {
                new Cart { IdCart = 1, IdUser = 2, Total = 2, CreatedAt = now, UpdateAt = now },
                new Cart { IdCart = 2, IdUser = 3, Total = 1, CreatedAt = now, UpdateAt = now },
                new Cart { IdCart = 3, IdUser = 4, Total = 3, CreatedAt = now, UpdateAt = now }
            };
            context.Carts.AddRange(carts);
            context.SaveChanges();

            var cartFoods = new CartFood[]
            {
                new CartFood { IdCartFood = 1, IdCart = 1, IdFood = 4, Quantity = 1, Price = 59000m, Total = 59000m, Note = "Ít cay" },
                new CartFood { IdCartFood = 2, IdCart = 1, IdFood = 2, Quantity = 1, Price = 55000m, Total = 55000m, Note = "Thêm cơm" },
                new CartFood { IdCartFood = 3, IdCart = 2, IdFood = 8, Quantity = 1, Price = 189000m, Total = 189000m, Note = "Nhiều rau" },
                new CartFood { IdCartFood = 4, IdCart = 3, IdFood = 10, Quantity = 2, Price = 89000m, Total = 178000m, Note = "Để riêng nước chấm" }
            };
            context.CartFoods.AddRange(cartFoods);
            context.SaveChanges();

            var drivers = new Driver[]
            {
                new Driver { IdDriver = 1, IdUser = 6, LicensePlate = "59A1-12345", Address = "Quận 1, TP.HCM", ExpRank = "Gold", DescStatus = "Giao nhanh khu vực trung tâm", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = false, RateAvg = 4.8m, TotalOrders = 320 },
                new Driver { IdDriver = 2, IdUser = 7, LicensePlate = "59B2-67890", Address = "Phú Nhuận, TP.HCM", ExpRank = "Silver", DescStatus = "Thân thiện, đúng giờ", CurrentLat = 10.801337m, CurrentLng = 106.714021m, IsBusy = true, RateAvg = 4.5m, TotalOrders = 210 }
            };
            context.Drivers.AddRange(drivers);
            context.SaveChanges();

            var orders = new Order[]
            {
                new Order { IdOrder = 1, IdUser = 2, IdRestaurant = 1, IdDriver = 1, IdVoucher = 1, OrderCode = "ORD000001", DeliveryAddress = "123 Nguyễn Trãi, Quận 1, TP.HCM", DeliveryLat = 10.762622m, DeliveryLng = 106.660172m, PaymentMethod = "cash", FoodAmount = 100000m, ShippingFee = 15000m, Discount = 10000m, FinalTotal = 105000m, PaymentStatus = "paid", Status = "completed", Note = "Giao nhanh giúp", CreatedAt = now.AddDays(-2), UpdatedAt = now.AddDays(-2).AddMinutes(45) },
                new Order { IdOrder = 2, IdUser = 3, IdRestaurant = 3, IdDriver = 2, IdVoucher = 2, OrderCode = "ORD000002", DeliveryAddress = "45 Lê Lợi, Quận 1, TP.HCM", DeliveryLat = 10.776889m, DeliveryLng = 106.700806m, PaymentMethod = "momo", FoodAmount = 189000m, ShippingFee = 20000m, Discount = 20000m, FinalTotal = 189000m, PaymentStatus = "paid", Status = "delivering", Note = "Gọi khi tới", CreatedAt = now.AddHours(-2), UpdatedAt = now.AddHours(-1) },
                new Order { IdOrder = 3, IdUser = 4, IdRestaurant = 4, IdDriver = null, IdVoucher = 3, OrderCode = "ORD000003", DeliveryAddress = "78 Phan Xích Long, Phú Nhuận, TP.HCM", DeliveryLat = 10.801337m, DeliveryLng = 106.714021m, PaymentMethod = "cash", FoodAmount = 178000m, ShippingFee = 18000m, Discount = 15000m, FinalTotal = 181000m, PaymentStatus = "unpaid", Status = "restaurant_accepted", Note = "Không lấy đũa", CreatedAt = now.AddHours(-1), UpdatedAt = now.AddMinutes(-50) }
            };
            context.Orders.AddRange(orders);
            context.SaveChanges();

            var orderFoods = new OrderFood[]
            {
                new OrderFood { IdOrderFood = 1, IdOrder = 1, IdFood = 1, Quantity = 1, UnitPrice = 45000m, TotalPrice = 45000m, Note = "" },
                new OrderFood { IdOrderFood = 2, IdOrder = 1, IdFood = 2, Quantity = 1, UnitPrice = 55000m, TotalPrice = 55000m, Note = "Thêm cơm" },
                new OrderFood { IdOrderFood = 3, IdOrder = 2, IdFood = 8, Quantity = 1, UnitPrice = 189000m, TotalPrice = 189000m, Note = "Nhiều rau" },
                new OrderFood { IdOrderFood = 4, IdOrder = 3, IdFood = 10, Quantity = 2, UnitPrice = 89000m, TotalPrice = 178000m, Note = "Để riêng nước chấm" }
            };
            context.OrderFoods.AddRange(orderFoods);
            context.SaveChanges();

            var reviews = new Review[]
            {
                new Review { IdReview = 1, IdUser = 2, IdOrder = 1, IdRestaurant = 1, FoodRating = 4.8f, DriverRating = 4.9f, CommentForRes = "Món ngon, đóng gói kỹ", CommentForShipper = "Giao nhanh", CreatedAt = now.AddDays(-2).AddHours(1) }
            };
            context.Reviews.AddRange(reviews);
            context.SaveChanges();

            var reviewFoods = new ReviewFood[]
            {
                new ReviewFood { IdReviewFood = 1, IdReview = 1, IdFood = 1, Rating = 4.7f, Comment = "Bánh giòn ngon", Image = "images/foods/Bánh xèo.jpg", Video = "" },
                new ReviewFood { IdReviewFood = 2, IdReview = 1, IdFood = 2, Rating = 4.8f, Comment = "Cá kho đậm vị", Image = "images/foods/Cá kho tộ.jpg", Video = "" }
            };
            context.ReviewFoods.AddRange(reviewFoods);
            context.SaveChanges();

            var notifications = new Notification[]
            {
                new Notification { IdNoti = 1, IdUser = 2, Title = "Đơn hàng hoàn tất", Body = "Đơn hàng #ORD000001 đã được giao thành công", OrderId = 1, IsRead = true, CreatedAt = now.AddDays(-2) },
                new Notification { IdNoti = 2, IdUser = 3, Title = "Đơn đang giao", Body = "Đơn hàng #ORD000002 đang trên đường giao", OrderId = 2, IsRead = false, CreatedAt = now.AddHours(-1) }
            };
            context.Notifications.AddRange(notifications);
            context.SaveChanges();

            var paymentMethods = new PaymentMethod[]
            {
                new PaymentMethod { IdTransaction = 1, IdUser = 2, IdOrder = 1, Method = "cash", Amount = 105000m },
                new PaymentMethod { IdTransaction = 2, IdUser = 3, IdOrder = 2, Method = "momo", Amount = 189000m },
                new PaymentMethod { IdTransaction = 3, IdUser = 4, IdOrder = 3, Method = "cash", Amount = 181000m }
            };
            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();
        }
    }
}
