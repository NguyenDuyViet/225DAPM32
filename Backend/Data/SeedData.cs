using Backend.Models;
using Microsoft.Extensions.Configuration;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using System.Net.Http;

namespace Backend.Data
{
    public static class SeedData
    {
        private static string UploadToCloudinary(Cloudinary cloudinary, string url, string folder, string name)
        {
            try
            {
                using var client = new HttpClient();
                var bytes = client.GetByteArrayAsync(url).GetAwaiter().GetResult();
                using var stream = new MemoryStream(bytes);
                
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(name + ".jpg", stream),
                    Folder = $"DAPM_32/{folder}",
                    Transformation = new Transformation().Quality("auto").FetchFormat("auto")
                };
                
                var result = cloudinary.Upload(uploadParams);
                if (result.Error == null && result.SecureUrl != null)
                {
                    return result.SecureUrl.ToString();
                }
            }
            catch
            {
                // Fallback to original unsplash URL if upload fails
            }
            return url;
        }

        public static void Initialize(AppDbContext context, bool forceReset = false)
        {
            // Only seed if we don't have 50 restaurants
            if (!forceReset && context.Restaurants.Count() >= 50) return;

            // Clear all tables to start fresh
            context.OrderFoods.RemoveRange(context.OrderFoods);
            context.OrderPromotions.RemoveRange(context.OrderPromotions);
            context.Orders.RemoveRange(context.Orders);
            context.CartFoods.RemoveRange(context.CartFoods);
            context.Carts.RemoveRange(context.Carts);
            context.ReviewFoods.RemoveRange(context.ReviewFoods);
            context.Reviews.RemoveRange(context.Reviews);
            context.Foods.RemoveRange(context.Foods);
            context.Promotions.RemoveRange(context.Promotions);
            context.Restaurants.RemoveRange(context.Restaurants);
            context.Drivers.RemoveRange(context.Drivers);
            context.Vouchers.RemoveRange(context.Vouchers);
            context.Notifications.RemoveRange(context.Notifications);
            context.PaymentMethods.RemoveRange(context.PaymentMethods);
            context.Complaints.RemoveRange(context.Complaints);
            context.Users.RemoveRange(context.Users);
            context.Roles.RemoveRange(context.Roles);
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();

            // Load Configuration to connect to Cloudinary
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var cloudName = config["Cloudinary:CloudName"];
            var apiKey = config["Cloudinary:ApiKey"];
            var apiSecret = config["Cloudinary:ApiSecret"];
            var account = new Account(cloudName, apiKey, apiSecret);
            var cloudinary = new Cloudinary(account);
            var defaultAvatarUrl = UploadToCloudinary(
                cloudinary,
                "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=300&auto=format&fit=crop&q=80",
                "users",
                "default-user");

            var now = DateTime.Now;

            // 1. Seed Roles
            var roles = new Backend.Models.Role[]
            {
                new Backend.Models.Role { IdRole = 1, RoleName = "admin" },
                new Backend.Models.Role { IdRole = 2, RoleName = "customer" },
                new Backend.Models.Role { IdRole = 3, RoleName = "restaurant" },
                new Backend.Models.Role { IdRole = 4, RoleName = "shipper" }
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();

            // 2. Seed Users
            var users = new User[]
            {
                new User { IdUser = 1, IdRole = 1, Username = "admin", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Quản trị viên", Phone = "0900000001", Email = "admin@example.com", Address = "Quận Hải Châu, Đà Nẵng", Avatar = "images/users/admin.jpg", Status = "active", CreatedAt = now, LastOnline = now, CurrentLat = 16.0678m, CurrentLng = 108.2208m, CancelRate = 0 },
                new User { IdUser = 2, IdRole = 2, Username = "customer01", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Nguyễn Văn An", Phone = "0901000001", Email = "customer01@example.com", Address = "123 Nguyễn Văn Linh, Quận Hải Châu, Đà Nẵng", Avatar = "images/users/customer01.jpg", Status = "active", CreatedAt = now, LastOnline = now, CurrentLat = 16.0678m, CurrentLng = 108.2208m, CancelRate = 0.02f },
                new User { IdUser = 3, IdRole = 2, Username = "customer02", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Trần Thị Bình", Phone = "0901000002", Email = "customer02@example.com", Address = "45 Điện Biên Phủ, Quận Thanh Khê, Đà Nẵng", Avatar = "images/users/customer02.jpg", Status = "active", CreatedAt = now, LastOnline = now.AddMinutes(-15), CurrentLat = 16.0620m, CurrentLng = 108.1950m, CancelRate = 0.01f },
                new User { IdUser = 4, IdRole = 2, Username = "customer03", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Lê Minh Châu", Phone = "0901000003", Email = "customer03@example.com", Address = "78 Võ Nguyên Giáp, Quận Sơn Trà, Đà Nẵng", Avatar = "images/users/customer03.jpg", Status = "active", CreatedAt = now, LastOnline = now.AddHours(-1), CurrentLat = 16.0754m, CurrentLng = 108.2435m, CancelRate = 0.04f },
                new User { IdUser = 5, IdRole = 3, Username = "restaurant01", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Chủ Nhà Hàng Ngon", Phone = "0902000001", Email = "restaurant01@example.com", Address = "100 Trần Phú, Quận Hải Châu, Đà Nẵng", Avatar = "images/users/restaurant.jpg", Status = "active", CreatedAt = now, LastOnline = now, CurrentLat = 16.0600m, CurrentLng = 108.2200m, CancelRate = 0 }
            };
            foreach (var user in users)
            {
                user.Avatar = defaultAvatarUrl;
            }
            context.Users.AddRange(users);
            context.SaveChanges();

            // 3. Cloudinary Image Seeding Pools (Unsplash Uploads)
            var restUnsplash = new string[]
            {
                "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?w=600&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1555396273-367ea4eb4db5?w=600&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1552566626-52f8b828add9?w=600&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1563245372-f21724e3856d?w=600&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1533777857889-4be7c70b33f7?w=600&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?w=600&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1555396273-367ea4eb4db5?w=600&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1495521821757-a1efb6729352?w=600&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1552566626-52f8b828add9?w=600&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1563245372-f21724e3856d?w=600&auto=format&fit=crop&q=80"
            };

            var foodUnsplash = new Dictionary<int, string[]>
            {
                { 1, new string[] { // Đồ ăn
                    "https://images.unsplash.com/photo-1582878826629-29b7ad1cdc43?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1512058564366-18510be2db19?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500&auto=format&fit=crop&q=80"
                }},
                { 2, new string[] { // Đồ uống
                    "https://images.unsplash.com/photo-1513530534585-c7b1394c6d51?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1621506289937-a8e4df240d0b?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1495521821757-a1efb6729352?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1621506289937-a8e4df240d0b?w=500&auto=format&fit=crop&q=80"
                }},
                { 3, new string[] { // Đồ chay
                    "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1540189549336-e6e99c3679fe?w=500&auto=format&fit=crop&q=80"
                }},
                { 4, new string[] { // Bánh kem
                    "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=500&auto=format&fit=crop&q=80"
                }},
                { 5, new string[] { // Tráng miệng
                    "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=500&auto=format&fit=crop&q=80"
                }},
                { 6, new string[] { // Món lẩu
                    "https://images.unsplash.com/photo-1552611052-33e04de081de?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1552611052-33e04de081de?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1552611052-33e04de081de?w=500&auto=format&fit=crop&q=80"
                }},
                { 7, new string[] { // Hải sản
                    "https://images.unsplash.com/photo-1519708227418-c8fd9a32b7a2?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1559827260-dc66d52bef19?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500&auto=format&fit=crop&q=80"
                }},
                { 8, new string[] { // Cơm hộp
                    "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=500&auto=format&fit=crop&q=80",
                    "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500&auto=format&fit=crop&q=80"
                }}
            };

            // Upload pools to Cloudinary and get Cloudinary URLs
            var restCloudinaryUrls = new string[restUnsplash.Length];
            for (int i = 0; i < restUnsplash.Length; i++)
            {
                restCloudinaryUrls[i] = UploadToCloudinary(cloudinary, restUnsplash[i], "restaurants", $"restaurant-pool-{i+1}");
            }

            var foodCloudinaryUrls = new Dictionary<int, string[]>();
            foreach (var kvp in foodUnsplash)
            {
                var urls = new string[kvp.Value.Length];
                for (int i = 0; i < kvp.Value.Length; i++)
                {
                    urls[i] = UploadToCloudinary(cloudinary, kvp.Value[i], "foods", $"food-pool-cat-{kvp.Key}-{i+1}");
                }
                foodCloudinaryUrls[kvp.Key] = urls;
            }

            // Categories
            var categories = new Category[]
            {
                new Category { IdCategory = 1, Name = "Đồ ăn", Icon = UploadToCloudinary(cloudinary, "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271413/DAPM_32/categories/category-1.jpg", "categories", "cat-1") },
                new Category { IdCategory = 2, Name = "Đồ uống", Icon = UploadToCloudinary(cloudinary, "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271414/DAPM_32/categories/category-2.jpg", "categories", "cat-2") },
                new Category { IdCategory = 3, Name = "Đồ chay", Icon = UploadToCloudinary(cloudinary, "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271415/DAPM_32/categories/category-3.jpg", "categories", "cat-3") },
                new Category { IdCategory = 4, Name = "Bánh kem", Icon = UploadToCloudinary(cloudinary, "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271416/DAPM_32/categories/category-4.jpg", "categories", "cat-4") },
                new Category { IdCategory = 5, Name = "Tráng miệng", Icon = UploadToCloudinary(cloudinary, "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271418/DAPM_32/categories/category-5.jpg", "categories", "cat-5") },
                new Category { IdCategory = 6, Name = "Món lẩu", Icon = UploadToCloudinary(cloudinary, "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271419/DAPM_32/categories/category-6.jpg", "categories", "cat-6") },
                new Category { IdCategory = 7, Name = "Hải sản", Icon = UploadToCloudinary(cloudinary, "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271419/DAPM_32/categories/category-7.jpg", "categories", "cat-7") },
                new Category { IdCategory = 8, Name = "Cơm hộp", Icon = UploadToCloudinary(cloudinary, "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271420/DAPM_32/categories/category-8.jpg", "categories", "cat-8") }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // 4. Seed 50 Restaurants distributed across all 8 districts of Da Nang
            var districtPool = new string[]
            {
                "Quận Cẩm Lệ",
                "Quận Hải Châu",
                "Quận Liên Chiểu",
                "Quận Ngũ Hành Sơn",
                "Quận Sơn Trà",
                "Quận Thanh Khê",
                "Huyện Hòa Vang",
                "Huyện Hoàng Sa"
            };

            var restaurantNames = new (string Name, string Desc, string Style)[]
            {
                ("Mỳ Quảng Bà Mua", "Đặc sản mỳ quảng tôm thịt ếch trứ danh", "noodle"),
                ("Bánh Tráng Cuốn Thịt Heo Quán Trần", "Thịt heo luộc hai đầu da cuốn bánh tráng đại lộc", "rice"),
                ("Bún Chả Cá Ông Tạ", "Bún chả cá thu, chả cá thác lác nước dùng thanh ngọt", "noodle"),
                ("Hải Sản Năm Đảnh", "Hải sản tươi ngon bình dân chế biến nóng sốt", "seafood"),
                ("Bê Thui Cầu Mống Mười", "Bê thui da giòn, thịt hồng ngọt ăn kèm mắm nêm cực cuốn", "rice"),
                ("Bún Bò Huế Bà Thương", "Bún bò huế chuẩn vị xưa đậm đà", "noodle"),
                ("Cơm Gà A Hải", "Cơm gà xối mỡ da giòn rụm", "rice"),
                ("Bánh Xèo Bà Dưỡng", "Bánh xèo nem lụi nước chấm gan xay siêu béo", "rice"),
                ("Quán Chè Liên", "Chè sầu liên nổi tiếng đà nẵng ngọt lịm thơm phức", "dessert"),
                ("Hải Sản Lão Đại", "Hải sản tươi sống chọn tại bể", "seafood"),
                ("Bún Mắm Vân", "Bún mắm nêm heo quay tai mui giòn sần sật", "noodle"),
                ("Bún Chả Cá Hờn", "Bún chả cá viên thơm nồng vị tiêu", "noodle"),
                ("Cơm Niêu Nhà Đỏ", "Cơm niêu giòn ngon ăn kèm cá kho tộ, canh cua rau đay", "rice"),
                ("Ẩm Thực Xèo", "Không gian ẩm thực miền trung đa dạng", "rice"),
                ("Nhà Hàng Cội Nguồn", "Các món ăn gia đình ba miền ấm cúng", "rice"),
                ("Bánh Canh Ruộng Cầu Thuận Phước", "Bánh canh cá nướng chả cá nóng hổi bình dân", "noodle"),
                ("Hải Sản Thơ Ý", "Nhà hàng hải sản ven biển mát mẻ thoáng đãng", "seafood"),
                ("Nem Lụi Bà Trai", "Nem lụi bọc sả thơm nức mũi", "rice"),
                ("Quán Ăn Gia Đình Tre Việt", "Đặc sản dân dã trong không gian tre nứa", "rice"),
                ("Bún Bò Bé Mai", "Bún bò bắp, chả cua, gân giòn ăn sáng đêm khuya", "noodle"),
                ("Mỳ Quảng Quê Hương", "Mỳ quảng gà ta trứng cút chuẩn vị quảng nam", "noodle"),
                ("Bún Chả Cá Nguyễn Chí Thanh", "Quán bún lâu đời nhất nhì đà thành", "noodle"),
                ("Cơm Gà Hồng Ngọc", "Cơm gà xé phay bóp hành tây thơm ngọt", "rice"),
                ("Bánh Mì Phượng Đà Nẵng", "Bánh mì giòn rụm pate bơ handmade ngập ngụa nhân", "rice"),
                ("Quán Nhậu Bình Dân 79", "Mồi nhậu đa dạng giá cả bình dân", "rice"),
                ("Lẩu Bò Sáu Hưng", "Lẩu đuôi bò hầm thuốc bắc đậm đà", "hotpot"),
                ("Hải Sản Bé Mặn", "Hải sản đà nẵng tươi rói ngắm biển sơn trà", "seafood"),
                ("Bún Mắm Nêm Bà Thuyên", "Bún mắm thịt heo quay da giòn", "noodle"),
                ("Cơm Tấm Diệu Hương", "Cơm tấm sườn bì chả thơm nức", "rice"),
                ("Bún Thịt Nướng Kim Anh", "Bún thịt nướng tương đậu phộng đặc sệt thơm béo", "noodle"),
                ("Bánh Cuốn Tiến Hưng", "Bánh cuốn nóng tráng mỏng ăn kèm chả bò đà nẵng", "rice"),
                ("Ẩm Thực Chay Hương Khách", "Món chay thanh tịnh, bổ dưỡng tốt cho sức khỏe", "vegetarian"),
                ("Nhà Hàng Chay Bồ Đề", "Thực đơn chay phong phú thanh đạm", "vegetarian"),
                ("Quán Chay Liên Hoa", "Điểm hẹn chay tịnh yên bình", "vegetarian"),
                ("Bánh Kem Anh Chi", "Bánh sinh nhật và bánh kem tươi thiết kế xinh xắn", "cake"),
                ("Tiệm Bánh BonPas Bakery", "Bánh mì ngọt, bánh lạnh phong cách châu âu", "cake"),
                ("Đồng Tiến Bakery", "Thương hiệu bánh mì bánh ngọt truyền thống đà nẵng", "cake"),
                ("Trà Sữa Gong Cha Đà Nẵng", "Trà sữa kem sữa béo ngậy nhập khẩu Đài Loan", "drink"),
                ("The Coffee House Nguyễn Văn Linh", "Không gian cà phê hiện đại trò chuyện làm việc", "drink"),
                ("Highlands Coffee Bạch Đằng", "Cà phê phin sữa đá view sông hàn thơ mộng", "drink"),
                ("Quán Cafe Cốc Đà Thành", "Cà phê muối đà nẵng đậm vị ngon tuyệt", "drink"),
                ("Mỳ Quảng Ếch Bếp Trang", "Mỳ quảng ếch để riêng trên mẹt sang trọng", "noodle"),
                ("Lẩu Băng Chuyền Kichi Kichi", "Đại tiệc lẩu nhúng không giới hạn cực đã", "hotpot"),
                ("GoGi House Đà Nẵng", "Thịt nướng Hàn Quốc xèo xèo chuẩn vị", "rice"),
                ("Hải Sản Bà Thôi", "Thương hiệu hải sản gia đình lâu đời uy tín", "seafood"),
                ("Quán Ốc Hút Đĩa Bay", "Ốc hút dừa, ốc đinh cay xè đặc trưng học sinh sinh viên", "seafood"),
                ("Bánh Tráng Kẹp Dì Hoa", "Bánh tráng kẹp pate trứng ướt, khô giòn rụm", "dessert"),
                ("Quán Kem Bơ Cô Vân", "Kem bơ chợ bắc mỹ an siêu dẻo mịn", "dessert"),
                ("Cơm Niêu Trúc Lâm Viên", "Cơm niêu sân vườn truyền thống tĩnh lặng", "rice"),
                ("Nhà Hàng Nổi Phố Cổ", "Thưởng thức ẩm thực sông nước độc đáo", "seafood")
            };

            var seedRestaurants = new List<Restaurant>();
            var random = new Random();

            for (int i = 0; i < 50; i++)
            {
                var rInfo = restaurantNames[i];
                var district = districtPool[i % districtPool.Length];
                var street = $"{random.Next(10, 299)} {GetRandomStreetName(random)}";
                
                // Coordinates distribution around district centroids in Da Nang
                decimal baseLat = 16.0678m;
                decimal baseLng = 108.2208m;
                switch (district)
                {
                    case "Quận Hải Châu": baseLat = 16.0678m; baseLng = 108.2208m; break;
                    case "Quận Thanh Khê": baseLat = 16.0620m; baseLng = 108.1950m; break;
                    case "Quận Sơn Trà": baseLat = 16.0754m; baseLng = 108.2435m; break;
                    case "Quận Ngũ Hành Sơn": baseLat = 16.0125m; baseLng = 108.2612m; break;
                    case "Quận Liên Chiểu": baseLat = 16.0712m; baseLng = 108.1495m; break;
                    case "Quận Cẩm Lệ": baseLat = 16.0224m; baseLng = 108.2045m; break;
                    case "Huyện Hòa Vang": baseLat = 15.9890m; baseLng = 108.1250m; break;
                    case "Huyện Hoàng Sa": baseLat = 16.1000m; baseLng = 108.2900m; break;
                }

                // Add small random noise to coordinates
                decimal lat = baseLat + (decimal)(random.NextDouble() - 0.5) * 0.015m;
                decimal lng = baseLng + (decimal)(random.NextDouble() - 0.5) * 0.015m;

                var imgUrl = restCloudinaryUrls[i % restCloudinaryUrls.Length];

                var res = new Restaurant
                {
                    IdRestaurant = i + 1,
                    NameRestaurant = rInfo.Name,
                    Description = rInfo.Desc,
                    Image = imgUrl,
                    Address = $"{street}, {district}, Đà Nẵng",
                    OpenTime = new TimeSpan(random.Next(6, 9), 0, 0),
                    CloseTime = new TimeSpan(random.Next(21, 23), 30, 0),
                    Lat = lat,
                    Lng = lng
                };
                seedRestaurants.Add(res);
            }
            context.Restaurants.AddRange(seedRestaurants);
            context.SaveChanges();

            // 5. Seed Foods (8-10 foods matching restaurant style, 2-3 drinks, optional combo)
            var seedFoods = new List<Food>();
            int foodIdCounter = 1;

            for (int rId = 1; rId <= 50; rId++)
            {
                var rInfo = restaurantNames[rId - 1];

                // Determine primary food category based on restaurant style
                int primaryCategory = 1; // Default to Đồ ăn
                if (rInfo.Style == "seafood") primaryCategory = 7;
                else if (rInfo.Style == "vegetarian") primaryCategory = 3;
                else if (rInfo.Style == "cake") primaryCategory = 4;
                else if (rInfo.Style == "dessert") primaryCategory = 5;
                else if (rInfo.Style == "drink") primaryCategory = 2;
                else if (rInfo.Style == "hotpot") primaryCategory = 6;
                else if (rInfo.Style == "rice") primaryCategory = 8;
                else if (rInfo.Style == "noodle") primaryCategory = 1; // Will generate noodle-specific names

                // Add 8-10 regular dishes - ONLY from primary category (matching restaurant specialty)
                int numRegularDishes = random.Next(8, 11);
                for (int f = 0; f < numRegularDishes; f++)
                {
                    var foodDetails = GetRestaurantSpecificFoodNameAndPrice(rInfo.Name, rInfo.Style, primaryCategory, f, random);
                    var imgPool = foodCloudinaryUrls.ContainsKey(primaryCategory) ? foodCloudinaryUrls[primaryCategory] : foodCloudinaryUrls[1];
                    var imgUrl = imgPool[random.Next(imgPool.Length)];

                    var food = new Food
                    {
                        IdFood = foodIdCounter++,
                        IdCategory = primaryCategory,
                        IdRestaurant = rId,
                        Name = foodDetails.Name,
                        Description = $"{foodDetails.Name} thơm ngon nóng hổi, đặc sản của {rInfo.Name}",
                        Image = imgUrl,
                        Price = foodDetails.Price,
                        Discount = random.Next(0, 5) == 0 ? foodDetails.Price * 0.1m : 0m,
                        CookCount = random.Next(20, 250),
                        PrepTime = random.Next(10, 30),
                        DailyQuantity = random.Next(20, 91)
                    };
                    seedFoods.Add(food);
                }

                // Add 2-3 drinks (Category 2) - same for all restaurants
                int numDrinks = random.Next(2, 4);
                for (int d = 0; d < numDrinks; d++)
                {
                    var drinkNames = new string[] { "Trà đá sả chanh", "Trà sữa trân châu", "Coca-Cola lạnh", "Pepsi lon", "Nước ép cam tươi", "Nước suối tinh khiết" };
                    var drinkPrices = new decimal[] { 15000m, 35000m, 20000m, 20000m, 30000m, 10000m };
                    int idx = (rId + d) % drinkNames.Length;

                    var imgPool = foodCloudinaryUrls[2];
                    var imgUrl = imgPool[random.Next(imgPool.Length)];

                    var drink = new Food
                    {
                        IdFood = foodIdCounter++,
                        IdCategory = 2,
                        IdRestaurant = rId,
                        Name = drinkNames[idx],
                        Description = $"{drinkNames[idx]} mát lạnh sảng khoái, giải nhiệt ngày hè cực đã.",
                        Image = imgUrl,
                        Price = drinkPrices[idx],
                        Discount = 0m,
                        CookCount = random.Next(50, 400),
                        PrepTime = 5,
                        DailyQuantity = random.Next(30, 121)
                    };
                    seedFoods.Add(drink);
                }

                // Add optional Combo (Category 1)
                if (random.Next(0, 2) == 0) // 50% chance of having a combo
                {
                    var comboPrice = random.Next(75, 130) * 1000m;
                    var comboImg = foodCloudinaryUrls[1][random.Next(foodCloudinaryUrls[1].Length)];

                    var combo = new Food
                    {
                        IdFood = foodIdCounter++,
                        IdCategory = 1,
                        IdRestaurant = rId,
                        Name = $"Combo Tiết Kiệm {rInfo.Name}",
                        Description = $"Trọn gói combo gồm các món đặc sản của quán kèm 1 nước ngọt mát lạnh với giá siêu hời!",
                        Image = comboImg,
                        Price = comboPrice,
                        Discount = comboPrice * 0.15m,
                        CookCount = random.Next(30, 180),
                        PrepTime = 20,
                        DailyQuantity = random.Next(15, 51)
                    };
                    seedFoods.Add(combo);
                }
            }
            context.Foods.AddRange(seedFoods);
            context.SaveChanges();

            // 6. Seed Drivers (Shippers) in Da Nang
            var shippers = new User[]
            {
                new User { IdUser = 6, IdRole = 4, Username = "shipper01", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Nguyễn Hữu Chiến", Phone = "0903000001", Email = "shipper01@example.com", Address = "Quận Cẩm Lệ, Đà Nẵng", Avatar = defaultAvatarUrl, Status = "active", CreatedAt = now, LastOnline = now, CurrentLat = 16.0224m, CurrentLng = 108.2045m, CancelRate = 0 },
                new User { IdUser = 7, IdRole = 4, Username = "shipper02", Password = BCrypt.Net.BCrypt.HashPassword("123456"), FullName = "Trần Lê Anh", Phone = "0903000002", Email = "shipper02@example.com", Address = "Quận Thanh Khê, Đà Nẵng", Avatar = defaultAvatarUrl, Status = "active", CreatedAt = now, LastOnline = now.AddMinutes(-5), CurrentLat = 16.0620m, CurrentLng = 108.1950m, CancelRate = 0.01f }
            };
            context.Users.AddRange(shippers);
            context.SaveChanges();

            var drivers = new Driver[]
            {
                new Driver { IdDriver = 1, IdUser = 6, LicensePlate = "43C1-99999", Address = "Quận Cẩm Lệ, Đà Nẵng", ExpRank = "Gold", DescStatus = "Giao nhanh, cẩn thận mọi nẻo đường Đà Nẵng", CurrentLat = 16.0224m, CurrentLng = 108.2045m, IsBusy = false, RateAvg = 4.9m, TotalOrders = 450 },
                new Driver { IdDriver = 2, IdUser = 7, LicensePlate = "43D2-88888", Address = "Quận Thanh Khê, Đà Nẵng", ExpRank = "Silver", DescStatus = "Đúng giờ, thân thiện, thông thuộc đường hẻm", CurrentLat = 16.0620m, CurrentLng = 108.1950m, IsBusy = true, RateAvg = 4.7m, TotalOrders = 280 }
            };
            context.Drivers.AddRange(drivers);
            context.SaveChanges();

            // 7. Seed Vouchers
            var vouchers = new Voucher[]
            {
                new Voucher { IdVoucher = 1, Code = "DANANG50", IdUser = 2, Value = 15000m, Expiry = now.AddDays(30), Used = false },
                new Voucher { IdVoucher = 2, Code = "MYQUANG10", IdUser = 3, Value = 10000m, Expiry = now.AddDays(14), Used = false },
                new Voucher { IdVoucher = 3, Code = "HAISAN20", IdUser = 4, Value = 25000m, Expiry = now.AddDays(20), Used = false }
            };
            context.Vouchers.AddRange(vouchers);
            context.SaveChanges();
        }

        private static string GetRandomStreetName(Random r)
        {
            var streets = new string[]
            {
                "Nguyễn Văn Linh", "Lê Duẩn", "Điện Biên Phủ", "Võ Nguyên Giáp", "Hoàng Sa",
                "Trần Hưng Đạo", "Bạch Đằng", "Phan Châu Trinh", "3 Tháng 2", "Nguyễn Hữu Thọ",
                "Ông Ích Đường", "Lê Đại Hành", "Ngô Quyền", "Phạm Văn Đồng", "Trần Phú"
            };
            return streets[r.Next(streets.Length)];
        }

        private static (string Name, decimal Price) GetRestaurantSpecificFoodNameAndPrice(string restaurantName, string style, int catId, int index, Random r)
        {
            // Generate food names SPECIFIC to restaurant style - no mixing
            switch (style.ToLower())
            {
                case "noodle": // Mỳ quảng, Bún chả cá, Bún bò, Bún thịt nướng, etc.
                    var noodleNames = new string[]
                    {
                        "Mỳ Quảng Thập Cẩm", "Mỳ Quảng Tôm Thịt", "Mỳ Quảng Ếch",
                        "Bún Chả Cá Đặc Biệt", "Bún Chả Cá Viên", "Bún Bò Thơm Nồng",
                        "Bún Thịt Nướng", "Bánh Canh Chả Cá", "Bánh Canh Cua"
                    };
                    return (noodleNames[index % noodleNames.Length], (r.Next(38, 68) * 1000m));

                case "rice": // Cơm gà, Cơm tấm, Bánh tráng, Bánh xèo, Bánh mì, etc.
                    var riceNames = new string[]
                    {
                        "Cơm Gà Xối Mỡ Da Giòn", "Cơm Gà Nước Mắm", "Cơm Tấm Sườn",
                        "Cơm Tấm Sườn Bì Chả", "Cơm Niêu Cá Kho", "Bánh Tráng Cuốn Thịt",
                        "Bánh Tráng Cuốn Nạm", "Bánh Xèo Giòn Rụm", "Bánh Mì Thơm",
                        "Bánh Mì Pate Gan"
                    };
                    return (riceNames[index % riceNames.Length], (r.Next(42, 72) * 1000m));

                case "seafood": // Tôm, cua, mực, ốc, chip chip, etc.
                    var seafoodNames = new string[]
                    {
                        "Tôm Nướng Muối Ớt", "Tôm Hùm Nướng Bơ", "Cua Sốt Me",
                        "Cua Hoàng Gia Hấp", "Mực Chiên Nước Mắm", "Mực Nướng Bơ Tỏi",
                        "Ốc Hương Xào Bơ", "Chip Chip Hấp Sả", "Ốc Đinh Cay Xè",
                        "Cá Nướng Muối Ớt"
                    };
                    return (seafoodNames[index % seafoodNames.Length], (r.Next(85, 190) * 1000m));

                case "hotpot": // Lẩu
                    var hotpotNames = new string[]
                    {
                        "Lẩu Bò Thái", "Lẩu Bò Thuốc Bắc", "Lẩu Ếch Măng Cay",
                        "Lẩu Đuôi Bò", "Lẩu Gà Ớt Hiểm", "Lẩu Hải Sản Chua Cay",
                        "Lẩu Tộ Mơ", "Lẩu Mắm Cua"
                    };
                    return (hotpotNames[index % hotpotNames.Length], (r.Next(160, 260) * 1000m));

                case "vegetarian": // Đồ chay
                    var vegNames = new string[]
                    {
                        "Cơm Chiên Dương Châu Chay", "Đậu Hũ Tứ Xuyên Chay", "Lẩu Nấm Chay",
                        "Mỳ Quảng Chay", "Nấm Bào Ngư Kho Tiêu", "Gỏi Ngó Sen Nấm",
                        "Rau Muống Chay", "Thịt Chay Nước Mắm"
                    };
                    return (vegNames[index % vegNames.Length], (r.Next(32, 58) * 1000m));

                case "cake": // Bánh kem
                    var cakeNames = new string[]
                    {
                        "Bánh Tiramisu Hộp", "Bánh Kem Trái Cây", "Bánh Mousse Xoài",
                        "Bánh Bông Lan Trứng Muối", "Bánh Kem Matcha", "Bánh Cheesecake",
                        "Bánh Kem Dâu", "Bánh Kem Chocolate"
                    };
                    return (cakeNames[index % cakeNames.Length], (r.Next(48, 185) * 1000m));

                case "dessert": // Tráng miệng
                    var dessertNames = new string[]
                    {
                        "Chè Sầu Liên", "Kem Bơ Dừa", "Caramel Đông Sương",
                        "Chè Thái Trái Cây", "Tàu Hũ Trân Châu", "Chè Chuối Nước Cot Dừa",
                        "Kem Bơ Choco", "Chè Ba Màu"
                    };
                    return (dessertNames[index % dessertNames.Length], (r.Next(22, 42) * 1000m));

                case "drink": // Đồ uống
                    var drinkSpecificNames = new string[]
                    {
                        "Trà Đen Đá", "Trà Xanh Đá", "Cà Phê Đen Đá",
                        "Cà Phê Sữa", "Sinh Tố Xoài", "Sinh Tố Dâu",
                        "Nước Lạnh Tự Nhiên", "Trà Nhãn Đá"
                    };
                    return (drinkSpecificNames[index % drinkSpecificNames.Length], (r.Next(15, 45) * 1000m));

                default:
                    return ("Món Ăn Đặc Sản", (r.Next(40, 70) * 1000m));
            }
        }
    }
}
