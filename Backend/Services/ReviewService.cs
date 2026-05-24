using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Repositories;

namespace Backend.Services
{
    public class ReviewService
    {
        private readonly ReviewRepository _reviewRepository;

        public ReviewService(ReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public Task<List<ReviewFoodResponse>> GetReviewFoodByIdFood(int idFood)
        {
            return _reviewRepository.GetReviewFoodByIdFood(idFood);
        }

        public Task<ReviewFoodResponse> CreateReviewFood(ReviewFoodRequest request, int idUser)
        {
            return _reviewRepository.CreateReviewFood(request, idUser);
        }
    }
}
