using Backend.Models;

namespace Backend.Repositories
{
    public class RestRepository : Repository<Restaurant>
    {
        public RestRepository(AppDbContext context) : base(context)
        { }
    }
}
