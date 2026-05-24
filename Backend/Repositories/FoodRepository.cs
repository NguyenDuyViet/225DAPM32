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

        public async Task<int> GetNextIdAsync()
        {
            return await _context.Foods.AnyAsync()
                ? await _context.Foods.MaxAsync(f => f.IdFood) + 1
                : 1;
        }
    }
}
