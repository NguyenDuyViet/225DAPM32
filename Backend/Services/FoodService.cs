using AutoMapper;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class FoodService
    {
        private readonly FoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public FoodService(FoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FoodResponse>> GetAllFoodsAsync()
        {
            var foods = await _foodRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FoodResponse>>(foods);
        }

        public async Task<FoodResponse?> GetFoodByIdAsync(int id)
        {
            var food = await _foodRepository.GetByIdAsync(id);
            if (food == null)
                return null;

            return _mapper.Map<FoodResponse>(food);
        }

        public async Task<FoodResponse> CreateFoodAsync(FoodRequest request)
        {
            var food = _mapper.Map<Food>(request);
            await _foodRepository.AddAsync(food);
            await _foodRepository.SaveChangesAsync();

            return _mapper.Map<FoodResponse>(food);
        }

        public async Task<FoodResponse?> UpdateFoodAsync(int id, FoodRequest request)
        {
            var existingFood = await _foodRepository.GetByIdAsync(id);
            if (existingFood == null)
                return null;

            _mapper.Map(request, existingFood);
            _foodRepository.Update(existingFood);
            await _foodRepository.SaveChangesAsync();

            return _mapper.Map<FoodResponse>(existingFood);
        }

        public async Task<bool> DeleteFoodAsync(int id)
        {
            var food = await _foodRepository.GetByIdAsync(id);
            if (food == null)
                return false;

            await _foodRepository.DeleteAsync(food);
            return await _foodRepository.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<FoodResponse>> GetFoodsByRestaurantAsync(int restaurantId)
        {
            var foods = await _foodRepository.FindAsync(f => f.IdRestaurant == restaurantId);
            return _mapper.Map<IEnumerable<FoodResponse>>(foods);
        }

        public async Task<IEnumerable<FoodResponse>> GetFoodsByCategoryAsync(int categoryId)
        {
            var foods = await _foodRepository.FindAsync(f => f.IdCategory == categoryId);
            return _mapper.Map<IEnumerable<FoodResponse>>(foods);
        }
    }
}
