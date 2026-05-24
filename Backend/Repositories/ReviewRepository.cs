using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ReviewRepository : Repository<ReviewFood>
    {
        public ReviewRepository (AppDbContext context) : base (context) { }

        public async Task<List<ReviewFoodResponse>> GetReviewFoodByIdFood(int idFood)
        {
            var list = await _context.ReviewFoods
                .Include(rv => rv.Review)
                    .ThenInclude(u => u.User)
                .Include(f => f.Food)
                .Where(rv => rv.IdFood == idFood)
                .Select(rf => new ReviewFoodResponse
                {
                    Username = rf.Review.User.Username,
                    UserAvatar = rf.Review.User.Avatar ?? string.Empty,
                    FoodName = rf.Food.Name,
                    Rating = rf.Rating,
                    Comment = rf.Comment,
                    Image = rf.Image,
                    Video = rf.Video,
                    CreatedAt = rf.Review.CreatedAt
                })
                .ToListAsync();
            
            return list;
        }

        public async Task<ReviewFoodResponse> CreateReviewFood(ReviewFoodRequest rq, int idUser)
        {
            var order = await _context.Orders
                       .Include(o => o.OrderFoods)
                       .FirstOrDefaultAsync(o =>
                           o.IdOrder == rq.IdOrder &&
                           o.IdUser == idUser &&
                           o.Status == "completed"
                       );
            if (order == null)
                throw new Exception("Đơn hàng không hợp lệ hoặc chưa hoàn thành.");

            var foodInOrder = order.OrderFoods.Any(of => of.IdFood == rq.IdFood);

            if (!foodInOrder)
                throw new Exception("Món ăn này không thuộc đơn hàng.");

            var review = await _context.Reviews
                        .Include(u => u.User)
                        .FirstOrDefaultAsync(rv =>
                            rv.IdOrder == rq.IdOrder &&
                            rv.IdUser == idUser
                        );

            if (review == null)
            {
                review = new Review
                {
                    IdReview = await GetNextReviewIdAsync(),
                    IdUser = idUser,
                    IdOrder = rq.IdOrder,
                    IdRestaurant = order.IdRestaurant,
                    FoodRating = rq.Rating,
                    DriverRating = 0,
                    CommentForRes = rq.Comment ?? string.Empty,
                    CommentForShipper = string.Empty,
                    CreatedAt = DateTime.Now
                };

                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();
            }

            var reviewFood = await _context.ReviewFoods
                            .Include(f => f.Food)
                            .FirstOrDefaultAsync(rf =>
                                rf.IdReview == review.IdReview &&
                                rf.IdFood == rq.IdFood
                            );

            if (reviewFood == null)
            {
                reviewFood = new ReviewFood
                {
                    IdReviewFood = await GetNextReviewFoodIdAsync(),
                    IdReview = review.IdReview,
                    IdFood = rq.IdFood,
                    Rating = rq.Rating,
                    Comment = rq.Comment,
                    Image = rq.Image,
                    Video = rq.Video
                };

                await _context.ReviewFoods.AddAsync(reviewFood);
            }
            else
            {
                reviewFood.Rating = rq.Rating;
                reviewFood.Comment = rq.Comment;
                reviewFood.Image = rq.Image;
                reviewFood.Video = rq.Video;
            }

            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(idUser);
            var food = await _context.Foods.FindAsync(rq.IdFood);

            return new ReviewFoodResponse
            {
                Username = user?.Username ?? "",
                UserAvatar = user?.Avatar ?? "",
                FoodName = food?.Name ?? "",
                Rating = reviewFood.Rating,
                Comment = reviewFood.Comment,
                Image = reviewFood.Image,
                Video = reviewFood.Video,
                CreatedAt = review.CreatedAt
            };
        }

        private async Task<int> GetNextReviewIdAsync()
        {
            return await _context.Reviews.AnyAsync()
                ? await _context.Reviews.MaxAsync(r => r.IdReview) + 1
                : 1;
        }

        private async Task<int> GetNextReviewFoodIdAsync()
        {
            return await _context.ReviewFoods.AnyAsync()
                ? await _context.ReviewFoods.MaxAsync(rf => rf.IdReviewFood) + 1
                : 1;
        }
    }
}
