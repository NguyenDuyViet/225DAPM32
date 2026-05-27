using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class CartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartResponse> GetCartByUserIdAsync(int userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            await NormalizeCartPricesAsync(cart);
            return ToCartResponse(cart);
        }

        public async Task<List<VoucherResponse>> GetAvailableVouchersAsync(int userId)
        {
            return await _context.Vouchers
                .Where(v => v.IdUser == userId && !v.Used && v.Expiry >= DateTime.Now)
                .OrderBy(v => v.Expiry)
                .Select(v => new VoucherResponse
                {
                    IdVoucher = v.IdVoucher,
                    Code = v.Code,
                    Value = v.Value,
                    Expiry = v.Expiry
                })
                .ToListAsync();
        }

        public async Task<CartResponse> AddItemAsync(int userId, CartItemRequest request)
        {
            if (request.Quantity <= 0)
                throw new InvalidOperationException("Số lượng món ăn phải lớn hơn 0.");

            var food = await _context.Foods
                .Include(f => f.Restaurant)
                .FirstOrDefaultAsync(f => f.IdFood == request.IdFood);

            if (food == null)
                throw new KeyNotFoundException("Không tìm thấy món ăn.");

            var cart = await GetOrCreateCartAsync(userId);
            var existingRestaurantId = cart.CartFoods.FirstOrDefault()?.Food.IdRestaurant;

            if (existingRestaurantId.HasValue && existingRestaurantId.Value != food.IdRestaurant)
                throw new InvalidOperationException("Giỏ hàng chỉ hỗ trợ món ăn từ cùng một nhà hàng.");

            var item = cart.CartFoods.FirstOrDefault(cf => cf.IdFood == request.IdFood);
            var resultingQuantity = (item?.Quantity ?? 0) + request.Quantity;

            if (food.DailyQuantity <= 0 || resultingQuantity > food.DailyQuantity)
                throw new InvalidOperationException("Món ăn không đủ số lượng để thêm vào giỏ hàng.");

            var price = GetSellingPrice(food);

            if (item == null)
            {
                item = new CartFood
                {
                    IdCartFood = await GetNextCartFoodIdAsync(),
                    IdCart = cart.IdCart,
                    IdFood = food.IdFood,
                    Quantity = request.Quantity,
                    Price = price,
                    Total = price * request.Quantity,
                    Note = request.Note ?? string.Empty
                };

                await _context.CartFoods.AddAsync(item);
                cart.CartFoods.Add(item);
            }
            else
            {
                item.Quantity += request.Quantity;
                item.Price = price;
                item.Total = item.Price * item.Quantity;
                item.Note = request.Note ?? item.Note;
            }

            await SaveCartAsync(cart);
            return await GetCartByUserIdAsync(userId);
        }

        public async Task<CartResponse> UpdateItemAsync(int userId, int idCartFood, UpdateCartItemRequest request)
        {
            var cart = await GetCartEntityByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException("Không tìm thấy giỏ hàng.");

            var item = cart.CartFoods.FirstOrDefault(cf => cf.IdCartFood == idCartFood);
            if (item == null)
                throw new KeyNotFoundException("Không tìm thấy món ăn trong giỏ hàng.");

            if (request.Quantity > item.Food.DailyQuantity)
                throw new InvalidOperationException("Món ăn không đủ số lượng để cập nhật giỏ hàng.");

            if (request.Quantity <= 0)
            {
                _context.CartFoods.Remove(item);
            }
            else
            {
                item.Quantity = request.Quantity;
                item.Total = item.Price * item.Quantity;
                item.Note = request.Note ?? item.Note;
            }

            await SaveCartAsync(cart);
            return await GetCartByUserIdAsync(userId);
        }

        public async Task<CartResponse> RemoveItemAsync(int userId, int idCartFood)
        {
            var cart = await GetCartEntityByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException("Không tìm thấy giỏ hàng.");

            var item = cart.CartFoods.FirstOrDefault(cf => cf.IdCartFood == idCartFood);
            if (item == null)
                throw new KeyNotFoundException("Không tìm thấy món ăn trong giỏ hàng.");

            _context.CartFoods.Remove(item);
            await SaveCartAsync(cart);

            return await GetCartByUserIdAsync(userId);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await GetCartEntityByUserIdAsync(userId);
            if (cart == null)
                return;

            _context.CartFoods.RemoveRange(cart.CartFoods);
            await SaveCartAsync(cart);
        }

        internal async Task<Cart?> GetCartEntityByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartFoods)
                    .ThenInclude(cf => cf.Food)
                        .ThenInclude(f => f.Restaurant)
                .FirstOrDefaultAsync(c => c.IdUser == userId);
        }

        internal async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await GetCartEntityByUserIdAsync(userId);
            if (cart != null)
                return cart;

            cart = new Cart
            {
                IdCart = await GetNextCartIdAsync(),
                IdUser = userId,
                Total = 0,
                CreatedAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CartFoods = new List<CartFood>()
            };

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        private async Task SaveCartAsync(Cart cart)
        {
            cart.Total = (int)cart.CartFoods.Sum(cf => cf.Total);
            cart.UpdateAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        private async Task NormalizeCartPricesAsync(Cart cart)
        {
            var changed = false;

            foreach (var item in cart.CartFoods)
            {
                if (item.Food == null)
                    continue;

                var price = GetSellingPrice(item.Food);
                var total = price * item.Quantity;

                if (item.Price != price || item.Total != total)
                {
                    item.Price = price;
                    item.Total = total;
                    changed = true;
                }
            }

            var cartTotal = (int)cart.CartFoods.Sum(cf => cf.Total);
            if (cart.Total != cartTotal)
            {
                cart.Total = cartTotal;
                changed = true;
            }

            if (changed)
            {
                cart.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        private CartResponse ToCartResponse(Cart cart)
        {
            var items = cart.CartFoods
                .Select(cf => new CartItemResponse
                {
                    IdCartFood = cf.IdCartFood,
                    IdFood = cf.IdFood,
                    FoodName = cf.Food.Name,
                    FoodImage = cf.Food.Image,
                    IdRestaurant = cf.Food.IdRestaurant,
                    RestaurantName = cf.Food.Restaurant.NameRestaurant,
                    Quantity = cf.Quantity,
                    Price = cf.Price,
                    Total = cf.Total,
                    Note = cf.Note
                })
                .ToList();

            return new CartResponse
            {
                IdCart = cart.IdCart,
                IdUser = cart.IdUser,
                TotalQuantity = items.Sum(i => i.Quantity),
                TotalAmount = items.Sum(i => i.Total),
                CreatedAt = cart.CreatedAt,
                UpdateAt = cart.UpdateAt,
                Items = items
            };
        }

        private static decimal GetSellingPrice(Food food)
        {
            return Math.Max(0, food.Price - (food.Discount ?? 0));
        }

        private async Task<int> GetNextCartIdAsync()
        {
            return await _context.Carts.AnyAsync()
                ? await _context.Carts.MaxAsync(c => c.IdCart) + 1
                : 1;
        }

        private async Task<int> GetNextCartFoodIdAsync()
        {
            return await _context.CartFoods.AnyAsync()
                ? await _context.CartFoods.MaxAsync(cf => cf.IdCartFood) + 1
                : 1;
        }
    }
}
