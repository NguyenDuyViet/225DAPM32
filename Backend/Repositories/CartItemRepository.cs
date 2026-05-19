using Backend.Models;

namespace Backend.Repositories
{
    public class CartItemRepository : Repository<CartFood>
    {
            public CartItemRepository(AppDbContext context) : base(context)
            {
        }
    }
}
