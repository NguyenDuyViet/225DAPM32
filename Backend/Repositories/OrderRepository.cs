using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository(AppDbContext context) : base(context)
        { }

        public async Task<List<Order>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            return await _dbSet
                .Where(o => o.IdRestaurant == restaurantId)
                .Include(o => o.User)
                .Include(o => o.Driver)
                    .ThenInclude(d => d.User)
                .Include(o => o.OrderFoods)
                    .ThenInclude(of => of.Food)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
