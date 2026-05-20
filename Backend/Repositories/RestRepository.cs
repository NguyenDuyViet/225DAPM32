using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RestRepository : Repository<Restaurant>
    {
        public RestRepository(AppDbContext context) : base(context)
        { }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<List<Restaurant>> GetPagedAsync(int page, int pageSize)
        {
            return await _dbSet
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(r => r.Foods.Any(f => f.IdCategory == categoryId))
                .CountAsync();
        }

        public async Task<List<Restaurant>> GetPagedByCategoryAsync(int categoryId, int page, int pageSize)
        {
            return await _dbSet
                .Where(r => r.Foods.Any(f => f.IdCategory == categoryId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
