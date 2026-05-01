using Backend.Models;

namespace Backend.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users.Any()) return; // DB has been seeded

            // Seed Roles
            var roles = new Role[]
            {
                new Role { IdRole = 1, Name = "customer", Description = "Khách hàng" },
                new Role { IdRole = 2, Name = "admin", Description = "Quản trị viên" },
                new Role { IdRole = 3, Name = "restaurant", Description = "Nhà hàng" },
                new Role { IdRole = 4, Name = "shipper", Description = "Người giao hàng" }
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();

            // Seed Users
            var users = new User[]
            {
                new User { IdUser = 1, Username = "user1", Password = "pass1", Phone = "0123456789", Email = "user1@example.com", Avatar = "avatar1.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 1", UpdateAvatar = "avatar1.jpg", UpdateBg = "bg1.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.1f, IdRole = 1 },
                new User { IdUser = 2, Username = "user2", Password = "pass2", Phone = "0987654321", Email = "user2@example.com", Avatar = "avatar2.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 2", UpdateAvatar = "avatar2.jpg", UpdateBg = "bg2.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.2f, IdRole = 1 },
                new User { IdUser = 3, Username = "user3", Password = "pass3", Phone = "0111111111", Email = "user3@example.com", Avatar = "avatar3.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 3", UpdateAvatar = "avatar3.jpg", UpdateBg = "bg3.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.0f, IdRole = 2 },
                new User { IdUser = 4, Username = "user4", Password = "pass4", Phone = "0222222222", Email = "user4@example.com", Avatar = "avatar4.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 4", UpdateAvatar = "avatar4.jpg", UpdateBg = "bg4.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.1f, IdRole = 3 },
                new User { IdUser = 5, Username = "user5", Password = "pass5", Phone = "0333333333", Email = "user5@example.com", Avatar = "avatar5.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 5", UpdateAvatar = "avatar5.jpg", UpdateBg = "bg5.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.3f, IdRole = 3 },
                new User { IdUser = 6, Username = "user6", Password = "pass6", Phone = "0444444444", Email = "user6@example.com", Avatar = "avatar6.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 6", UpdateAvatar = "avatar6.jpg", UpdateBg = "bg6.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.0f, IdRole = 4 },
                new User { IdUser = 7, Username = "user7", Password = "pass7", Phone = "0555555555", Email = "user7@example.com", Avatar = "avatar7.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 7", UpdateAvatar = "avatar7.jpg", UpdateBg = "bg7.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.1f, IdRole = 4 },
                new User { IdUser = 8, Username = "user8", Password = "pass8", Phone = "0666666666", Email = "user8@example.com", Avatar = "avatar8.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 8", UpdateAvatar = "avatar8.jpg", UpdateBg = "bg8.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.2f, IdRole = 1 },
                new User { IdUser = 9, Username = "user9", Password = "pass9", Phone = "0777777777", Email = "user9@example.com", Avatar = "avatar9.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 9", UpdateAvatar = "avatar9.jpg", UpdateBg = "bg9.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.0f, IdRole = 3 },
                new User { IdUser = 10, Username = "user10", Password = "pass10", Phone = "0888888888", Email = "user10@example.com", Avatar = "avatar10.jpg", Status = "active", CreatedAt = DateTime.Now, UpdateBio = "Bio 10", UpdateAvatar = "avatar10.jpg", UpdateBg = "bg10.jpg", CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.1f, IdRole = 4 }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            // Seed Addresses
            var addresses = new Address[]
            {
                new Address { IdAddress = 1, IdUser = 1, Name = "Nguyen Van A", Phone = "0123456789", AddressDetail = "123 Nguyen Trai, District 1", Lat = 10.762622m, Lng = 106.660172m, Note = "Near the park", IsDefault = true },
                new Address { IdAddress = 2, IdUser = 2, Name = "Tran Thi B", Phone = "0987654321", AddressDetail = "456 Le Loi, District 3", Lat = 10.762622m, Lng = 106.660172m, Note = "Apartment 5B", IsDefault = true },
                new Address { IdAddress = 3, IdUser = 3, Name = "Le Van C", Phone = "0111111111", AddressDetail = "789 Tran Hung Dao, District 5", Lat = 10.762622m, Lng = 106.660172m, Note = "Office building", IsDefault = true },
                new Address { IdAddress = 4, IdUser = 4, Name = "Pham Thi D", Phone = "0222222222", AddressDetail = "101 Vo Van Tan, District 3", Lat = 10.762622m, Lng = 106.660172m, Note = "House with red gate", IsDefault = true },
                new Address { IdAddress = 5, IdUser = 5, Name = "Hoang Van E", Phone = "0333333333", AddressDetail = "202 Nguyen Thi Minh Khai, District 1", Lat = 10.762622m, Lng = 106.660172m, Note = "Near supermarket", IsDefault = true },
                new Address { IdAddress = 6, IdUser = 6, Name = "Vo Thi F", Phone = "0444444444", AddressDetail = "303 Dien Bien Phu, District 3", Lat = 10.762622m, Lng = 106.660172m, Note = "Villa area", IsDefault = true },
                new Address { IdAddress = 7, IdUser = 7, Name = "Do Van G", Phone = "0555555555", AddressDetail = "404 Cach Mang Thang 8, District 10", Lat = 10.762622m, Lng = 106.660172m, Note = "Near hospital", IsDefault = true },
                new Address { IdAddress = 8, IdUser = 8, Name = "Bui Thi H", Phone = "0666666666", AddressDetail = "505 Nguyen Trai, District 5", Lat = 10.762622m, Lng = 106.660172m, Note = "Residential area", IsDefault = true },
                new Address { IdAddress = 9, IdUser = 9, Name = "Nguyen Van I", Phone = "0777777777", AddressDetail = "606 Le Hong Phong, District 10", Lat = 10.762622m, Lng = 106.660172m, Note = "Near school", IsDefault = true },
                new Address { IdAddress = 10, IdUser = 10, Name = "Tran Thi J", Phone = "0888888888", AddressDetail = "707 Tran Phu, District 5", Lat = 10.762622m, Lng = 106.660172m, Note = "Commercial area", IsDefault = true }
            };
            context.Addresses.AddRange(addresses);
            context.SaveChanges();

            // Seed Categories
            var categories = new Category[]
            {
                new Category { IdCategory = 1, Name = "Vietnamese Food", Icon = "vietnamese.png" },
                new Category { IdCategory = 2, Name = "Fast Food", Icon = "fastfood.png" },
                new Category { IdCategory = 3, Name = "Chinese Food", Icon = "chinese.png" },
                new Category { IdCategory = 4, Name = "Japanese Food", Icon = "japanese.png" },
                new Category { IdCategory = 5, Name = "Italian Food", Icon = "italian.png" },
                new Category { IdCategory = 6, Name = "Desserts", Icon = "desserts.png" },
                new Category { IdCategory = 7, Name = "Beverages", Icon = "beverages.png" },
                new Category { IdCategory = 8, Name = "Seafood", Icon = "seafood.png" },
                new Category { IdCategory = 9, Name = "Vegetarian", Icon = "vegetarian.png" },
                new Category { IdCategory = 10, Name = "Street Food", Icon = "streetfood.png" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Restaurants
            var restaurants = new Restaurant[]
            {
                new Restaurant { IdRestaurant = 1, NameRestaurant = "Pho 24", Description = "Traditional Vietnamese noodle soup", Image = "pho24.jpg", Address = "123 Nguyen Trai St", OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
                new Restaurant { IdRestaurant = 2, NameRestaurant = "Pizza Hut", Description = "American style pizza", Image = "pizzahut.jpg", Address = "456 Le Loi St", OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(23, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
                new Restaurant { IdRestaurant = 3, NameRestaurant = "Dim Sum House", Description = "Authentic Chinese dim sum", Image = "dimsum.jpg", Address = "789 Tran Hung Dao St", OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(20, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
                new Restaurant { IdRestaurant = 4, NameRestaurant = "Sushi Express", Description = "Fresh Japanese sushi", Image = "sushi.jpg", Address = "101 Vo Van Tan St", OpenTime = new TimeSpan(11, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
                new Restaurant { IdRestaurant = 5, NameRestaurant = "Pasta Fresca", Description = "Italian pasta dishes", Image = "pasta.jpg", Address = "202 Nguyen Thi Minh Khai St", OpenTime = new TimeSpan(12, 0, 0), CloseTime = new TimeSpan(23, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
                new Restaurant { IdRestaurant = 6, NameRestaurant = "Sweet Dreams", Description = "Delicious desserts", Image = "desserts.jpg", Address = "303 Dien Bien Phu St", OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(21, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
                new Restaurant { IdRestaurant = 7, NameRestaurant = "Juice Bar", Description = "Fresh fruit juices", Image = "juice.jpg", Address = "404 Cach Mang Thang 8 St", OpenTime = new TimeSpan(7, 0, 0), CloseTime = new TimeSpan(20, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
                new Restaurant { IdRestaurant = 8, NameRestaurant = "Seafood Paradise", Description = "Fresh seafood dishes", Image = "seafood.jpg", Address = "505 Nguyen Trai St", OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
                new Restaurant { IdRestaurant = 9, NameRestaurant = "Green Garden", Description = "Vegetarian cuisine", Image = "vegetarian.jpg", Address = "606 Le Hong Phong St", OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(21, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
                new Restaurant { IdRestaurant = 10, NameRestaurant = "Street Eats", Description = "Authentic street food", Image = "streetfood.jpg", Address = "707 Tran Phu St", OpenTime = new TimeSpan(16, 0, 0), CloseTime = new TimeSpan(24, 0, 0), Lat = 10.762622m, Lng = 106.660172m }
            };
            context.Restaurants.AddRange(restaurants);
            context.SaveChanges();

            // Seed Foods
            var foods = new Food[]
            {
                new Food { IdFood = 1, IdCategory = 1, IdRestaurant = 1, Name = "Pho Bo", Description = "Beef noodle soup", Image = "pho_bo.jpg", Price = 30.00m, Discount = 5.00m, CookCount = 100, PrepTime = 15 },
                new Food { IdFood = 2, IdCategory = 1, IdRestaurant = 1, Name = "Bun Cha", Description = "Grilled pork with noodles", Image = "bun_cha.jpg", Price = 25.00m, Discount = 3.00m, CookCount = 80, PrepTime = 10 },
                new Food { IdFood = 3, IdCategory = 2, IdRestaurant = 2, Name = "Margherita Pizza", Description = "Classic cheese pizza", Image = "margherita.jpg", Price = 45.00m, Discount = 10.00m, CookCount = 50, PrepTime = 20 },
                new Food { IdFood = 4, IdCategory = 2, IdRestaurant = 2, Name = "Chicken Burger", Description = "Grilled chicken burger", Image = "chicken_burger.jpg", Price = 35.00m, Discount = 5.00m, CookCount = 70, PrepTime = 15 },
                new Food { IdFood = 5, IdCategory = 3, IdRestaurant = 3, Name = "Steamed Dumplings", Description = "Pork dumplings", Image = "dumplings.jpg", Price = 20.00m, Discount = 2.00m, CookCount = 60, PrepTime = 12 },
                new Food { IdFood = 6, IdCategory = 4, IdRestaurant = 4, Name = "California Roll", Description = "Crab and avocado roll", Image = "california_roll.jpg", Price = 40.00m, Discount = 8.00m, CookCount = 40, PrepTime = 10 },
                new Food { IdFood = 7, IdCategory = 5, IdRestaurant = 5, Name = "Carbonara", Description = "Creamy pasta with bacon", Image = "carbonara.jpg", Price = 50.00m, Discount = 10.00m, CookCount = 30, PrepTime = 18 },
                new Food { IdFood = 8, IdCategory = 6, IdRestaurant = 6, Name = "Chocolate Cake", Description = "Rich chocolate dessert", Image = "chocolate_cake.jpg", Price = 25.00m, Discount = 5.00m, CookCount = 90, PrepTime = 5 },
                new Food { IdFood = 9, IdCategory = 7, IdRestaurant = 7, Name = "Orange Juice", Description = "Fresh squeezed orange juice", Image = "orange_juice.jpg", Price = 15.00m, Discount = 2.00m, CookCount = 120, PrepTime = 2 },
                new Food { IdFood = 10, IdCategory = 8, IdRestaurant = 8, Name = "Grilled Salmon", Description = "Fresh grilled salmon", Image = "grilled_salmon.jpg", Price = 60.00m, Discount = 12.00m, CookCount = 25, PrepTime = 20 }
            };
            context.Foods.AddRange(foods);
            context.SaveChanges();

            // Seed Carts
            var carts = new Cart[]
            {
                new Cart { IdCart = 1, IdUser = 1, Total = 2, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now },
                new Cart { IdCart = 2, IdUser = 2, Total = 1, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now },
                new Cart { IdCart = 3, IdUser = 3, Total = 3, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now },
                new Cart { IdCart = 4, IdUser = 4, Total = 1, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now },
                new Cart { IdCart = 5, IdUser = 5, Total = 2, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now },
                new Cart { IdCart = 6, IdUser = 6, Total = 1, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now },
                new Cart { IdCart = 7, IdUser = 7, Total = 4, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now },
                new Cart { IdCart = 8, IdUser = 8, Total = 2, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now },
                new Cart { IdCart = 9, IdUser = 9, Total = 1, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now },
                new Cart { IdCart = 10, IdUser = 10, Total = 3, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now }
            };
            context.Carts.AddRange(carts);
            context.SaveChanges();

            // Seed CartFoods
            var cartFoods = new CartFood[]
            {
                new CartFood { IdCartFood = 1, IdCart = 1, IdFood = 1, Quantity = 2, Note = "Extra spicy" },
                new CartFood { IdCartFood = 2, IdCart = 1, IdFood = 2, Quantity = 1, Note = "No onions" },
                new CartFood { IdCartFood = 3, IdCart = 2, IdFood = 3, Quantity = 1, Note = "Extra cheese" },
                new CartFood { IdCartFood = 4, IdCart = 3, IdFood = 4, Quantity = 2, Note = "No pickles" },
                new CartFood { IdCartFood = 5, IdCart = 3, IdFood = 5, Quantity = 1, Note = "Vegetarian" },
                new CartFood { IdCartFood = 6, IdCart = 4, IdFood = 6, Quantity = 1, Note = "Wasabi on side" },
                new CartFood { IdCartFood = 7, IdCart = 5, IdFood = 7, Quantity = 1, Note = "Less salt" },
                new CartFood { IdCartFood = 8, IdCart = 5, IdFood = 8, Quantity = 1, Note = "No nuts" },
                new CartFood { IdCartFood = 9, IdCart = 6, IdFood = 9, Quantity = 1, Note = "Extra ice" },
                new CartFood { IdCartFood = 10, IdCart = 7, IdFood = 10, Quantity = 2, Note = "Well done" }
            };
            context.CartFoods.AddRange(cartFoods);
            context.SaveChanges();

            // Seed Drivers
            var drivers = new Driver[]
            {
                new Driver { IdDriver = 1, IdUser = 1, LicensePlate = "51A-12345", Address = "123 Driver St", ExpRank = "Gold", DescStatus = "Experienced driver", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = false, RateAvg = 4.5m, TotalOrders = 150 },
                new Driver { IdDriver = 2, IdUser = 2, LicensePlate = "51B-67890", Address = "456 Driver Ave", ExpRank = "Silver", DescStatus = "Reliable delivery", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = true, RateAvg = 4.2m, TotalOrders = 120 },
                new Driver { IdDriver = 3, IdUser = 3, LicensePlate = "51C-11111", Address = "789 Driver Blvd", ExpRank = "Bronze", DescStatus = "New but enthusiastic", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = false, RateAvg = 3.8m, TotalOrders = 50 },
                new Driver { IdDriver = 4, IdUser = 4, LicensePlate = "51D-22222", Address = "101 Driver Ln", ExpRank = "Gold", DescStatus = "Top rated driver", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = false, RateAvg = 4.8m, TotalOrders = 200 },
                new Driver { IdDriver = 5, IdUser = 5, LicensePlate = "51E-33333", Address = "202 Driver Rd", ExpRank = "Silver", DescStatus = "Consistent performance", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = true, RateAvg = 4.0m, TotalOrders = 90 },
                new Driver { IdDriver = 6, IdUser = 6, LicensePlate = "51F-44444", Address = "303 Driver Way", ExpRank = "Bronze", DescStatus = "Learning the ropes", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = false, RateAvg = 3.5m, TotalOrders = 30 },
                new Driver { IdDriver = 7, IdUser = 7, LicensePlate = "51G-55555", Address = "404 Driver Ct", ExpRank = "Gold", DescStatus = "Veteran driver", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = false, RateAvg = 4.7m, TotalOrders = 180 },
                new Driver { IdDriver = 8, IdUser = 8, LicensePlate = "51H-66666", Address = "505 Driver Pl", ExpRank = "Silver", DescStatus = "Good with customers", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = true, RateAvg = 4.3m, TotalOrders = 110 },
                new Driver { IdDriver = 9, IdUser = 9, LicensePlate = "51I-77777", Address = "606 Driver Sq", ExpRank = "Bronze", DescStatus = "Eager to please", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = false, RateAvg = 3.9m, TotalOrders = 60 },
                new Driver { IdDriver = 10, IdUser = 10, LicensePlate = "51J-88888", Address = "707 Driver Cir", ExpRank = "Gold", DescStatus = "Highly recommended", CurrentLat = 10.762622m, CurrentLng = 106.660172m, IsBusy = false, RateAvg = 4.6m, TotalOrders = 160 }
            };
            context.Drivers.AddRange(drivers);
            context.SaveChanges();

            // Seed Orders
            var orders = new Order[]
            {
                new Order { IdOrder = 1, IdUser = 1, IdAddress = 1, IdDriver = 1, PaymentMethod = "Cash", Total = 55.00m, ShippingFee = 5.00m, Discount = 5.00m, FinalTotal = 55.00m, Status = "completed", Note = "Please deliver quickly", CreatedAt = DateTime.Now.AddDays(-1), ConfirmedAt = DateTime.Now.AddDays(-1).AddMinutes(10), DeliveringAt = DateTime.Now.AddDays(-1).AddMinutes(20), DeliveredAt = DateTime.Now.AddDays(-1).AddMinutes(40) },
                new Order { IdOrder = 2, IdUser = 2, IdAddress = 2, IdDriver = 2, PaymentMethod = "Card", Total = 45.00m, ShippingFee = 5.00m, Discount = 0.00m, FinalTotal = 50.00m, Status = "delivering", Note = "Ring doorbell", CreatedAt = DateTime.Now.AddHours(-2), ConfirmedAt = DateTime.Now.AddHours(-2).AddMinutes(5), DeliveringAt = DateTime.Now.AddHours(-1) },
                new Order { IdOrder = 3, IdUser = 3, IdAddress = 3, IdDriver = 3, PaymentMethod = "Cash", Total = 75.00m, ShippingFee = 5.00m, Discount = 10.00m, FinalTotal = 70.00m, Status = "confirmed", Note = "Extra napkins", CreatedAt = DateTime.Now.AddHours(-1), ConfirmedAt = DateTime.Now.AddHours(-1).AddMinutes(15) },
                new Order { IdOrder = 4, IdUser = 4, IdAddress = 4, IdDriver = 4, PaymentMethod = "Card", Total = 40.00m, ShippingFee = 5.00m, Discount = 8.00m, FinalTotal = 37.00m, Status = "pending", Note = "No contact delivery", CreatedAt = DateTime.Now.AddMinutes(-30) },
                new Order { IdOrder = 5, IdUser = 5, IdAddress = 5, IdDriver = 5, PaymentMethod = "Cash", Total = 65.00m, ShippingFee = 5.00m, Discount = 5.00m, FinalTotal = 65.00m, Status = "completed", Note = "Leave at door", CreatedAt = DateTime.Now.AddDays(-2), ConfirmedAt = DateTime.Now.AddDays(-2).AddMinutes(8), DeliveringAt = DateTime.Now.AddDays(-2).AddMinutes(15), DeliveredAt = DateTime.Now.AddDays(-2).AddMinutes(35) },
                new Order { IdOrder = 6, IdUser = 6, IdAddress = 6, IdDriver = 6, PaymentMethod = "Card", Total = 15.00m, ShippingFee = 5.00m, Discount = 2.00m, FinalTotal = 18.00m, Status = "completed", Note = "Cold drink", CreatedAt = DateTime.Now.AddHours(-3), ConfirmedAt = DateTime.Now.AddHours(-3).AddMinutes(5), DeliveringAt = DateTime.Now.AddHours(-3).AddMinutes(10), DeliveredAt = DateTime.Now.AddHours(-3).AddMinutes(20) },
                new Order { IdOrder = 7, IdUser = 7, IdAddress = 7, IdDriver = 7, PaymentMethod = "Cash", Total = 120.00m, ShippingFee = 5.00m, Discount = 12.00m, FinalTotal = 113.00m, Status = "delivering", Note = "Large order", CreatedAt = DateTime.Now.AddHours(-1), ConfirmedAt = DateTime.Now.AddHours(-1).AddMinutes(10), DeliveringAt = DateTime.Now.AddMinutes(-30) },
                new Order { IdOrder = 8, IdUser = 8, IdAddress = 8, IdDriver = 8, PaymentMethod = "Card", Total = 60.00m, ShippingFee = 5.00m, Discount = 0.00m, FinalTotal = 65.00m, Status = "confirmed", Note = "Fresh fish", CreatedAt = DateTime.Now.AddMinutes(-45), ConfirmedAt = DateTime.Now.AddMinutes(-40) },
                new Order { IdOrder = 9, IdUser = 9, IdAddress = 9, IdDriver = 9, PaymentMethod = "Cash", Total = 25.00m, ShippingFee = 5.00m, Discount = 5.00m, FinalTotal = 25.00m, Status = "pending", Note = "Vegetarian meal", CreatedAt = DateTime.Now.AddMinutes(-15) },
                new Order { IdOrder = 10, IdUser = 10, IdAddress = 10, IdDriver = 10, PaymentMethod = "Card", Total = 85.00m, ShippingFee = 5.00m, Discount = 10.00m, FinalTotal = 80.00m, Status = "completed", Note = "Birthday surprise", CreatedAt = DateTime.Now.AddDays(-1), ConfirmedAt = DateTime.Now.AddDays(-1).AddMinutes(12), DeliveringAt = DateTime.Now.AddDays(-1).AddMinutes(25), DeliveredAt = DateTime.Now.AddDays(-1).AddMinutes(50) }
            };
            context.Orders.AddRange(orders);
            context.SaveChanges();

            // Seed OrderFoods
            var orderFoods = new OrderFood[]
            {
                new OrderFood { IdOrderFood = 1, IdOrder = 1, IdFood = 1, Quantity = 1, UnitPrice = 30.00m, Note = "Extra beef" },
                new OrderFood { IdOrderFood = 2, IdOrder = 1, IdFood = 2, Quantity = 1, UnitPrice = 25.00m, Note = "Spicy" },
                new OrderFood { IdOrderFood = 3, IdOrder = 2, IdFood = 3, Quantity = 1, UnitPrice = 45.00m, Note = "Thin crust" },
                new OrderFood { IdOrderFood = 4, IdOrder = 3, IdFood = 4, Quantity = 2, UnitPrice = 35.00m, Note = "No mayo" },
                new OrderFood { IdOrderFood = 5, IdOrder = 4, IdFood = 6, Quantity = 1, UnitPrice = 40.00m, Note = "Extra wasabi" },
                new OrderFood { IdOrderFood = 6, IdOrder = 5, IdFood = 7, Quantity = 1, UnitPrice = 50.00m, Note = "Less cream" },
                new OrderFood { IdOrderFood = 7, IdOrder = 5, IdFood = 8, Quantity = 1, UnitPrice = 25.00m, Note = "Chocolate sauce" },
                new OrderFood { IdOrderFood = 8, IdOrder = 6, IdFood = 9, Quantity = 1, UnitPrice = 15.00m, Note = "No sugar" },
                new OrderFood { IdOrderFood = 9, IdOrder = 7, IdFood = 10, Quantity = 2, UnitPrice = 60.00m, Note = "Medium rare" },
                new OrderFood { IdOrderFood = 10, IdOrder = 8, IdFood = 5, Quantity = 1, UnitPrice = 20.00m, Note = "Vegetarian" }
            };
            context.OrderFoods.AddRange(orderFoods);
            context.SaveChanges();

            // Seed Reviews
            var reviews = new Review[]
            {
                new Review { IdReview = 1, IdUser = 1, IdOrder = 1, IdRestaurant = 1, FoodRating = 4.5f, DriverRating = 4.0f, CommentForRes = "Great pho!", CommentForShipper = "Fast delivery", CreatedAt = DateTime.Now.AddDays(-1).AddHours(1) },
                new Review { IdReview = 2, IdUser = 2, IdOrder = 2, IdRestaurant = 2, FoodRating = 4.0f, DriverRating = 4.5f, CommentForRes = "Good pizza", CommentForShipper = "Friendly driver", CreatedAt = DateTime.Now.AddHours(-1) },
                new Review { IdReview = 3, IdUser = 3, IdOrder = 3, IdRestaurant = 2, FoodRating = 3.5f, DriverRating = 4.0f, CommentForRes = "Burgers were okay", CommentForShipper = "On time", CreatedAt = DateTime.Now.AddMinutes(-30) },
                new Review { IdReview = 4, IdUser = 4, IdOrder = 4, IdRestaurant = 4, FoodRating = 4.8f, DriverRating = 4.2f, CommentForRes = "Amazing sushi", CommentForShipper = "Careful packaging", CreatedAt = DateTime.Now.AddMinutes(-15) },
                new Review { IdReview = 5, IdUser = 5, IdOrder = 5, IdRestaurant = 5, FoodRating = 4.2f, DriverRating = 3.8f, CommentForRes = "Tasty pasta", CommentForShipper = "Good service", CreatedAt = DateTime.Now.AddDays(-2).AddHours(1) },
                new Review { IdReview = 6, IdUser = 6, IdOrder = 6, IdRestaurant = 7, FoodRating = 4.0f, DriverRating = 4.5f, CommentForRes = "Fresh juice", CommentForShipper = "Quick delivery", CreatedAt = DateTime.Now.AddHours(-2) },
                new Review { IdReview = 7, IdUser = 7, IdOrder = 7, IdRestaurant = 8, FoodRating = 4.7f, DriverRating = 4.0f, CommentForRes = "Excellent salmon", CommentForShipper = "Professional", CreatedAt = DateTime.Now.AddMinutes(-45) },
                new Review { IdReview = 8, IdUser = 8, IdOrder = 8, IdRestaurant = 3, FoodRating = 3.8f, DriverRating = 4.3f, CommentForRes = "Good dumplings", CommentForShipper = "Nice person", CreatedAt = DateTime.Now.AddMinutes(-20) },
                new Review { IdReview = 9, IdUser = 9, IdOrder = 9, IdRestaurant = 9, FoodRating = 4.5f, DriverRating = 4.0f, CommentForRes = "Healthy food", CommentForShipper = "Reliable", CreatedAt = DateTime.Now.AddMinutes(-10) },
                new Review { IdReview = 10, IdUser = 10, IdOrder = 10, IdRestaurant = 6, FoodRating = 4.3f, DriverRating = 4.8f, CommentForRes = "Delicious cake", CommentForShipper = "Great attitude", CreatedAt = DateTime.Now.AddDays(-1).AddHours(2) }
            };
            context.Reviews.AddRange(reviews);
            context.SaveChanges();

            // Seed Notifications
            var notifications = new Notification[]
            {
                new Notification { IdNoti = 1, IdUser = 1, Title = "Order Confirmed", Body = "Your order #1 has been confirmed", OrderId = 1, IsRead = true, CreatedAt = DateTime.Now.AddDays(-1) },
                new Notification { IdNoti = 2, IdUser = 2, Title = "Order Out for Delivery", Body = "Your order #2 is on the way", OrderId = 2, IsRead = false, CreatedAt = DateTime.Now.AddHours(-1) },
                new Notification { IdNoti = 3, IdUser = 3, Title = "Order Ready", Body = "Your order #3 is ready for pickup", OrderId = 3, IsRead = true, CreatedAt = DateTime.Now.AddMinutes(-30) },
                new Notification { IdNoti = 4, IdUser = 4, Title = "New Promotion", Body = "Check out our new sushi deals!", IsRead = false, CreatedAt = DateTime.Now.AddHours(-2) },
                new Notification { IdNoti = 5, IdUser = 5, Title = "Order Delivered", Body = "Your order #5 has been delivered", OrderId = 5, IsRead = true, CreatedAt = DateTime.Now.AddDays(-2) },
                new Notification { IdNoti = 6, IdUser = 6, Title = "Payment Successful", Body = "Payment for order #6 received", OrderId = 6, IsRead = true, CreatedAt = DateTime.Now.AddHours(-3) },
                new Notification { IdNoti = 7, IdUser = 7, Title = "Driver Assigned", Body = "Driver assigned to order #7", OrderId = 7, IsRead = false, CreatedAt = DateTime.Now.AddMinutes(-45) },
                new Notification { IdNoti = 8, IdUser = 8, Title = "Restaurant Update", Body = "New menu items available", IsRead = false, CreatedAt = DateTime.Now.AddHours(-1) },
                new Notification { IdNoti = 9, IdUser = 9, Title = "Order Reminder", Body = "Don't forget your order #9", OrderId = 9, IsRead = true, CreatedAt = DateTime.Now.AddMinutes(-10) },
                new Notification { IdNoti = 10, IdUser = 10, Title = "Review Request", Body = "How was your recent order?", OrderId = 10, IsRead = false, CreatedAt = DateTime.Now.AddDays(-1) }
            };
            context.Notifications.AddRange(notifications);
            context.SaveChanges();

            // Seed Vouchers
            var vouchers = new Voucher[]
            {
                new Voucher { IdVoucher = 1, Code = "WELCOME10", IdUser = 1, Value = 10.00m, Expiry = DateTime.Now.AddDays(30), Used = false },
                new Voucher { IdVoucher = 2, Code = "PIZZA20", IdUser = 2, Value = 20.00m, Expiry = DateTime.Now.AddDays(15), Used = false },
                new Voucher { IdVoucher = 3, Code = "SUSHI15", IdUser = 4, Value = 15.00m, Expiry = DateTime.Now.AddDays(20), Used = false },
                new Voucher { IdVoucher = 4, Code = "PASTA25", IdUser = 5, Value = 25.00m, Expiry = DateTime.Now.AddDays(10), Used = true },
                new Voucher { IdVoucher = 5, Code = "DESSERT5", IdUser = 6, Value = 5.00m, Expiry = DateTime.Now.AddDays(25), Used = false },
                new Voucher { IdVoucher = 6, Code = "SEAFOOD30", IdUser = 7, Value = 30.00m, Expiry = DateTime.Now.AddDays(7), Used = false },
                new Voucher { IdVoucher = 7, Code = "VEG10", IdUser = 9, Value = 10.00m, Expiry = DateTime.Now.AddDays(14), Used = false },
                new Voucher { IdVoucher = 8, Code = "STREETFOOD8", IdUser = 10, Value = 8.00m, Expiry = DateTime.Now.AddDays(12), Used = false },
                new Voucher { IdVoucher = 9, Code = "CHINESE12", IdUser = 3, Value = 12.00m, Expiry = DateTime.Now.AddDays(18), Used = false },
                new Voucher { IdVoucher = 10, Code = "JAPANESE18", IdUser = 8, Value = 18.00m, Expiry = DateTime.Now.AddDays(22), Used = false }
            };
            context.Vouchers.AddRange(vouchers);
            context.SaveChanges();

            // Seed PaymentMethods
            var paymentMethods = new PaymentMethod[]
            {
                new PaymentMethod { IdTransaction = 1, IdUser = 1, IdOrder = 1, Method = "Cash", Amount = 55.00m },
                new PaymentMethod { IdTransaction = 2, IdUser = 2, IdOrder = 2, Method = "Credit Card", Amount = 50.00m },
                new PaymentMethod { IdTransaction = 3, IdUser = 3, IdOrder = 3, Method = "Cash", Amount = 70.00m },
                new PaymentMethod { IdTransaction = 4, IdUser = 4, IdOrder = 4, Method = "Debit Card", Amount = 37.00m },
                new PaymentMethod { IdTransaction = 5, IdUser = 5, IdOrder = 5, Method = "Cash", Amount = 65.00m },
                new PaymentMethod { IdTransaction = 6, IdUser = 6, IdOrder = 6, Method = "Credit Card", Amount = 18.00m },
                new PaymentMethod { IdTransaction = 7, IdUser = 7, IdOrder = 7, Method = "Cash", Amount = 113.00m },
                new PaymentMethod { IdTransaction = 8, IdUser = 8, IdOrder = 8, Method = "Debit Card", Amount = 65.00m },
                new PaymentMethod { IdTransaction = 9, IdUser = 9, IdOrder = 9, Method = "Cash", Amount = 25.00m },
                new PaymentMethod { IdTransaction = 10, IdUser = 10, IdOrder = 10, Method = "Credit Card", Amount = 80.00m }
            };
            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();
        }
    }
}