using Backend.Models;

namespace Backend.Repositories
{
    public class FoodRepository : Repository<Food> 
    {
        public FoodRepository(AppDbContext context) : base(context)
        {
        }
    }
}
