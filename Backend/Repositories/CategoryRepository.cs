using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public new async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.Foods)
                .ToListAsync();
        }

        public new async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Foods)
                .FirstOrDefaultAsync(c => c.IdCategory == id);
        }

        public async Task<int> GetNextIdAsync()
        {
            return await _context.Categories.AnyAsync()
                ? await _context.Categories.MaxAsync(c => c.IdCategory) + 1
                : 1;
        }
    }
}
