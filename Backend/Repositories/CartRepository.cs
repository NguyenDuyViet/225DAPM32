using Backend.Models;

namespace Backend.Repositories
{
    public class CartRepository : Repository<Cart>
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }
    }
}