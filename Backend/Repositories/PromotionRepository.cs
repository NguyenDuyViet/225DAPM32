using Backend.Models;

namespace Backend.Repositories
{
    public class PromotionRepository : Repository<Promotion> 
    {
        public PromotionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
