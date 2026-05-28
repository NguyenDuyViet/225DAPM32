using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class FoodRepository : Repository<Food>
    {
        public FoodRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Food>> GetAllWithDetailsAsync()
        {
            return await _context.Foods
                .Include(f => f.Category)
                .Include(f => f.Restaurant)
                .ToListAsync();
        }

        public async Task<Food?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Foods
                .Include(f => f.Category)
                .Include(f => f.Restaurant)
                .FirstOrDefaultAsync(f => f.IdFood == id);
        }

        public async Task<Dictionary<int, int>> GetCompletedSoldQuantitiesAsync(IEnumerable<int> foodIds)
        {
            var ids = foodIds.Distinct().ToList();
            if (ids.Count == 0)
                return new Dictionary<int, int>();

            return await _context.OrderFoods
                .Where(orderFood => ids.Contains(orderFood.IdFood) && orderFood.Order.Status == "completed")
                .GroupBy(orderFood => orderFood.IdFood)
                .Select(group => new { IdFood = group.Key, Quantity = group.Sum(orderFood => orderFood.Quantity) })
                .ToDictionaryAsync(item => item.IdFood, item => item.Quantity);
        }

        public async Task<int> GetNextIdAsync()
        {
            return await _context.Foods.AnyAsync()
                ? await _context.Foods.MaxAsync(f => f.IdFood) + 1
                : 1;
        }
    }
}
