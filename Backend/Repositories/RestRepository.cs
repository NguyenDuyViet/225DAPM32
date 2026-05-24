using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RestRepository : Repository<Restaurant>
    {
        public RestRepository(AppDbContext context) : base(context)
        { }

        public async Task<int> CountAsync(int? categoryId = null, string? search = null, string? district = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            IQueryable<Restaurant> query = _dbSet;

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(r => r.Foods.Any(f => f.IdCategory == categoryId));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.Trim().ToLower();
                query = query.Where(r => r.NameRestaurant.ToLower().Contains(lowerSearch) || 
                                         r.Foods.Any(f => f.Name.ToLower().Contains(lowerSearch)));
            }

            if (!string.IsNullOrWhiteSpace(district))
            {
                query = query.Where(r => r.Address.Contains(district));
            }

            if (minPrice.HasValue || maxPrice.HasValue)
            {
                query = query.Where(r => r.Foods.Any(f => 
                    (!minPrice.HasValue || f.Price >= minPrice.Value) && 
                    (!maxPrice.HasValue || f.Price <= maxPrice.Value)
                ));
            }

            return await query.CountAsync();
        }

        public async Task<List<Restaurant>> GetPagedAsync(int page, int pageSize, int? categoryId = null, string? search = null, string? district = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            IQueryable<Restaurant> query = _dbSet;

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(r => r.Foods.Any(f => f.IdCategory == categoryId));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.Trim().ToLower();
                query = query.Where(r => r.NameRestaurant.ToLower().Contains(lowerSearch) || 
                                         r.Foods.Any(f => f.Name.ToLower().Contains(lowerSearch)));
            }

            if (!string.IsNullOrWhiteSpace(district))
            {
                query = query.Where(r => r.Address.Contains(district));
            }

            if (minPrice.HasValue || maxPrice.HasValue)
            {
                query = query.Where(r => r.Foods.Any(f => 
                    (!minPrice.HasValue || f.Price >= minPrice.Value) && 
                    (!maxPrice.HasValue || f.Price <= maxPrice.Value)
                ));
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountByCategoryAsync(int categoryId)
        {
            return await CountAsync(categoryId);
        }

        public async Task<List<Restaurant>> GetPagedByCategoryAsync(int categoryId, int page, int pageSize)
        {
            return await GetPagedAsync(page, pageSize, categoryId);
        }
    }
}
