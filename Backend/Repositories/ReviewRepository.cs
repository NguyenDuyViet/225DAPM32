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

        public async Task SubmitOrderReview(OrderReviewRequest request, int idUser)
        {
            ValidateRating(request.RestaurantRating, "nhà hàng");
            ValidateRating(request.DriverRating, "shipper");

            var order = await _context.Orders
                .Include(o => o.OrderFoods)
                .FirstOrDefaultAsync(o =>
                    o.IdOrder == request.IdOrder &&
                    o.IdUser == idUser &&
                    o.Status == "completed");

            if (order == null)
                throw new InvalidOperationException("Chỉ có thể đánh giá đơn hàng đã giao thành công.");

            if (order.IdDriver == null)
                throw new InvalidOperationException("Đơn hàng chưa có thông tin shipper để đánh giá.");

            var review = await _context.Reviews
                .Include(r => r.ReviewFoods)
                .FirstOrDefaultAsync(r => r.IdOrder == request.IdOrder && r.IdUser == idUser);

            if (review == null)
            {
                review = new Review
                {
                    IdReview = await GetNextReviewIdAsync(),
                    IdUser = idUser,
                    IdOrder = order.IdOrder,
                    IdRestaurant = order.IdRestaurant,
                    CreatedAt = DateTime.Now,
                    ReviewFoods = new List<ReviewFood>()
                };
                await _context.Reviews.AddAsync(review);
            }

            review.FoodRating = request.RestaurantRating;
            review.DriverRating = request.DriverRating;
            review.CommentForRes = request.CommentForRestaurant?.Trim() ?? string.Empty;
            review.CommentForShipper = request.CommentForShipper?.Trim() ?? string.Empty;
            review.ReviewFoods ??= new List<ReviewFood>();

            var foodIdsInOrder = order.OrderFoods.Select(item => item.IdFood).ToHashSet();
            if (!foodIdsInOrder.SetEquals(request.Foods.Select(item => item.IdFood)))
                throw new InvalidOperationException("Vui lòng đánh giá đầy đủ các món ăn trong đơn hàng.");

            var nextReviewFoodId = await GetNextReviewFoodIdAsync();
            foreach (var foodReview in request.Foods)
            {
                if (!foodIdsInOrder.Contains(foodReview.IdFood))
                    throw new InvalidOperationException("Món ăn đánh giá không thuộc đơn hàng này.");

                ValidateRating(foodReview.Rating, "món ăn");
                var existing = review.ReviewFoods.FirstOrDefault(item => item.IdFood == foodReview.IdFood);
                if (existing == null)
                {
                    existing = new ReviewFood
                    {
                        IdReviewFood = nextReviewFoodId++,
                        IdReview = review.IdReview,
                        IdFood = foodReview.IdFood
                    };
                    review.ReviewFoods.Add(existing);
                    await _context.ReviewFoods.AddAsync(existing);
                }

                existing.Rating = foodReview.Rating;
                existing.Comment = foodReview.Comment?.Trim();
            }

            await _context.SaveChangesAsync();
        }

        private static void ValidateRating(float rating, string subject)
        {
            if (rating < 1 || rating > 5)
                throw new InvalidOperationException($"Điểm đánh giá {subject} phải từ 1 đến 5.");
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
