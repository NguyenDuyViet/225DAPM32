using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly CartService _cartService;

        public OrderService(AppDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<List<OrderResponse>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.Driver)
                    .ThenInclude(d => d.User)
                .Include(o => o.OrderFoods)
                    .ThenInclude(of => of.Food)
                .Where(o => o.IdUser == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(ToOrderResponse).ToList();
        }

        public async Task<List<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await GetOrdersQuery()
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(ToOrderResponse).ToList();
        }

        public async Task<List<OrderResponse>> GetOrdersByRestaurantAsync(int idRestaurant)
        {
            var orders = await GetOrdersQuery()
                .Where(o => o.IdRestaurant == idRestaurant)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(ToOrderResponse).ToList();
        }

        public async Task<List<OrderResponse>> GetOrdersByDriverAsync(int idDriver, bool history)
        {
            var statuses = history
                ? new[] { "completed", "cancelled", "canceled" }
                : new[] { "confirmed", "delivering", "restaurant_accepted" };

            var orders = await GetOrdersQuery()
                .Where(o => o.IdDriver == idDriver && statuses.Contains(o.Status))
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(ToOrderResponse).ToList();
        }

        public async Task<OrderResponse> GetOrderByIdAsync(int userId, int idOrder)
        {
            var order = await _context.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.Driver)
                    .ThenInclude(d => d.User)
                .Include(o => o.OrderFoods)
                    .ThenInclude(of => of.Food)
                .FirstOrDefaultAsync(o => o.IdOrder == idOrder && o.IdUser == userId);

            if (order == null)
                throw new KeyNotFoundException("Không tìm thấy đơn hàng.");

            return ToOrderResponse(order);
        }

        public async Task<OrderResponse> CreateOrderFromCartAsync(int userId, CreateOrderRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DeliveryAddress))
                throw new InvalidOperationException("Địa chỉ giao hàng không được để trống.");

            var cart = await _cartService.GetCartEntityByUserIdAsync(userId);
            if (cart == null || !cart.CartFoods.Any())
                throw new InvalidOperationException("Giỏ hàng đang trống.");

            var restaurantIds = cart.CartFoods.Select(cf => cf.Food.IdRestaurant).Distinct().ToList();
            if (restaurantIds.Count != 1)
                throw new InvalidOperationException("Một đơn hàng chỉ hỗ trợ món ăn từ cùng một nhà hàng.");

            var foodAmount = cart.CartFoods.Sum(cf => cf.Total);
            var discount = await GetVoucherDiscountAsync(userId, request.IdVoucher, foodAmount);
            var now = DateTime.Now;
            var idOrder = await GetNextOrderIdAsync();

            var order = new Order
            {
                IdOrder = idOrder,
                IdUser = userId,
                IdRestaurant = restaurantIds[0],
                IdVoucher = request.IdVoucher,
                OrderCode = $"ORD{idOrder:000000}",
                DeliveryAddress = request.DeliveryAddress.Trim(),
                DeliveryLat = request.DeliveryLat,
                DeliveryLng = request.DeliveryLng,
                PaymentMethod = request.PaymentMethod,
                FoodAmount = foodAmount,
                ShippingFee = request.ShippingFee,
                Discount = discount,
                FinalTotal = Math.Max(0, foodAmount + request.ShippingFee - discount),
                PaymentStatus = request.PaymentMethod.Equals("cash", StringComparison.OrdinalIgnoreCase) ? "unpaid" : "pending",
                Status = "pending",
                Note = request.Note,
                CreatedAt = now,
                UpdatedAt = now,
                OrderFoods = new List<OrderFood>()
            };

            var nextOrderFoodId = await GetNextOrderFoodIdAsync();
            foreach (var cartFood in cart.CartFoods)
            {
                order.OrderFoods.Add(new OrderFood
                {
                    IdOrderFood = nextOrderFoodId++,
                    IdOrder = idOrder,
                    IdFood = cartFood.IdFood,
                    Quantity = cartFood.Quantity,
                    UnitPrice = cartFood.Price,
                    TotalPrice = cartFood.Total,
                    Note = cartFood.Note
                });
            }

            await _context.Orders.AddAsync(order);

            if (request.IdVoucher.HasValue)
            {
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.IdVoucher == request.IdVoucher.Value);
                if (voucher != null)
                    voucher.Used = true;
            }

            _context.CartFoods.RemoveRange(cart.CartFoods);
            cart.Total = 0;
            cart.UpdateAt = now;

            await _context.SaveChangesAsync();

            return await GetOrderByIdAsync(userId, idOrder);
        }

        public async Task<OrderResponse> CancelOrderAsync(int userId, int idOrder, CancelOrderRequest request)
        {
            var order = await _context.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.OrderFoods)
                    .ThenInclude(of => of.Food)
                .FirstOrDefaultAsync(o => o.IdOrder == idOrder && o.IdUser == userId);

            if (order == null)
                throw new KeyNotFoundException("Không tìm thấy đơn hàng.");

            if (order.Status is "completed" or "cancelled")
                throw new InvalidOperationException("Không thể hủy đơn hàng ở trạng thái hiện tại.");

            order.Status = "cancelled";
            order.CancelReason = request.CancelReason;
            order.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return ToOrderResponse(order);
        }

        public async Task<OrderResponse> UpdateOrderStatusAsync(int idOrder, UpdateOrderStatusRequest request)
        {
            var order = await GetOrdersQuery().FirstOrDefaultAsync(o => o.IdOrder == idOrder);
            if (order == null)
                throw new KeyNotFoundException("Không tìm thấy đơn hàng.");

            if (string.IsNullOrWhiteSpace(request.Status))
                throw new InvalidOperationException("Trạng thái đơn hàng không được để trống.");

            order.Status = NormalizeStatus(request.Status);
            order.UpdatedAt = DateTime.Now;

            if (request.IdDriver.HasValue)
            {
                var driver = await _context.Drivers.FindAsync(request.IdDriver.Value);
                if (driver == null)
                    throw new KeyNotFoundException("Không tìm thấy shipper.");

                order.IdDriver = request.IdDriver.Value;
                driver.IsBusy = order.Status is "delivering" or "confirmed";
            }

            if (order.Status is "cancelled" or "canceled")
                order.CancelReason = request.CancelReason;

            if (order.Status == "completed" && order.Driver != null)
            {
                order.PaymentStatus = "paid";
                order.Driver.IsBusy = false;
                order.Driver.TotalOrders += 1;
            }

            await _context.SaveChangesAsync();
            return ToOrderResponse(order);
        }

        private async Task<decimal> GetVoucherDiscountAsync(int userId, int? idVoucher, decimal foodAmount)
        {
            if (!idVoucher.HasValue)
                return 0;

            var voucher = await _context.Vouchers.FirstOrDefaultAsync(v =>
                v.IdVoucher == idVoucher.Value &&
                v.IdUser == userId &&
                !v.Used &&
                v.Expiry >= DateTime.Now);

            if (voucher == null)
                throw new InvalidOperationException("Voucher không hợp lệ hoặc đã hết hạn.");

            return Math.Min(voucher.Value, foodAmount);
        }

        private static OrderResponse ToOrderResponse(Order order)
        {
            return new OrderResponse
            {
                IdOrder = order.IdOrder,
                OrderCode = order.OrderCode,
                IdUser = order.IdUser,
                IdRestaurant = order.IdRestaurant,
                RestaurantName = order.Restaurant.NameRestaurant,
                DeliveryAddress = order.DeliveryAddress,
                DeliveryLat = order.DeliveryLat,
                DeliveryLng = order.DeliveryLng,
                PaymentMethod = order.PaymentMethod,
                FoodAmount = order.FoodAmount,
                Total = order.FoodAmount,
                ShippingFee = order.ShippingFee,
                Discount = order.Discount,
                FinalTotal = order.FinalTotal,
                PaymentStatus = order.PaymentStatus,
                Status = order.Status,
                Note = order.Note,
                CancelReason = order.CancelReason,
                DriverName = order.Driver?.User?.FullName,
                DriverPhone = order.Driver?.User?.Phone,
                EstimatedDelivery = EstimateDelivery(order),
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = order.OrderFoods.Select(of => new OrderItemResponse
                {
                    IdOrderFood = of.IdOrderFood,
                    IdFood = of.IdFood,
                    FoodName = of.Food.Name,
                    FoodImage = of.Food.Image,
                    Quantity = of.Quantity,
                    UnitPrice = of.UnitPrice,
                    TotalPrice = of.TotalPrice,
                    Note = of.Note
                }).ToList()
            };
        }

        private IQueryable<Order> GetOrdersQuery()
        {
            return _context.Orders
                .Include(o => o.Restaurant)
                .Include(o => o.Driver)
                    .ThenInclude(d => d.User)
                .Include(o => o.OrderFoods)
                    .ThenInclude(of => of.Food);
        }

        private static DateTime? EstimateDelivery(Order order)
        {
            return order.Status is "completed" or "cancelled" or "canceled"
                ? order.UpdatedAt
                : order.CreatedAt.AddMinutes(45);
        }

        private static string NormalizeStatus(string status)
        {
            return status.Trim().ToLowerInvariant() switch
            {
                "canceled" => "cancelled",
                "accepted" => "confirmed",
                "restaurant_accepted" => "confirmed",
                var value => value
            };
        }

        private async Task<int> GetNextOrderIdAsync()
        {
            return await _context.Orders.AnyAsync()
                ? await _context.Orders.MaxAsync(o => o.IdOrder) + 1
                : 1;
        }

        private async Task<int> GetNextOrderFoodIdAsync()
        {
            return await _context.OrderFoods.AnyAsync()
                ? await _context.OrderFoods.MaxAsync(of => of.IdOrderFood) + 1
                : 1;
        }
    }
}
