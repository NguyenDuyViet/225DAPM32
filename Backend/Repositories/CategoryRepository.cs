using Backend.Models;

namespace Backend.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
