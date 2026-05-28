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
            var foods = await _foodRepository.GetAllWithDetailsAsync();
            return await MapWithSoldQuantitiesAsync(foods);
        }

        public async Task<FoodResponse?> GetFoodByIdAsync(int id)
        {
            var food = await _foodRepository.GetByIdWithDetailsAsync(id);
            if (food == null)
                return null;

            return (await MapWithSoldQuantitiesAsync(new[] { food })).Single();
        }

        public async Task<FoodResponse> CreateFoodAsync(FoodRequest request)
        {
            var food = _mapper.Map<Food>(request);
            food.IdFood = await _foodRepository.GetNextIdAsync();
            await _foodRepository.AddAsync(food);
            await _foodRepository.SaveChangesAsync();

            var createdFood = await _foodRepository.GetByIdWithDetailsAsync(food.IdFood) ?? food;
            return (await MapWithSoldQuantitiesAsync(new[] { createdFood })).Single();
        }

        public async Task<FoodResponse?> UpdateFoodAsync(int id, FoodRequest request)
        {
            var existingFood = await _foodRepository.GetByIdAsync(id);
            if (existingFood == null)
                return null;

            _mapper.Map(request, existingFood);
            _foodRepository.Update(existingFood);
            await _foodRepository.SaveChangesAsync();

            var updatedFood = await _foodRepository.GetByIdWithDetailsAsync(existingFood.IdFood) ?? existingFood;
            return (await MapWithSoldQuantitiesAsync(new[] { updatedFood })).Single();
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
            return await MapWithSoldQuantitiesAsync(foods);
        }

        public async Task<IEnumerable<FoodResponse>> GetFoodsByCategoryAsync(int categoryId)
        {
            var foods = await _foodRepository.FindAsync(f => f.IdCategory == categoryId);
            return await MapWithSoldQuantitiesAsync(foods);
        }

        public async Task<FoodResponse?> UpdateDailyQuantityAsync(int foodId, int quantity)
        {
            var existingFood = await _foodRepository.GetByIdAsync(foodId);
            if (existingFood == null)
                return null;

            existingFood.DailyQuantity = quantity;
            _foodRepository.Update(existingFood);
            await _foodRepository.SaveChangesAsync();

            return (await MapWithSoldQuantitiesAsync(new[] { existingFood })).Single();
        }

        private async Task<List<FoodResponse>> MapWithSoldQuantitiesAsync(IEnumerable<Food> foods)
        {
            var foodList = foods.ToList();
            var responses = _mapper.Map<List<FoodResponse>>(foodList);
            var soldQuantities = await _foodRepository.GetCompletedSoldQuantitiesAsync(foodList.Select(food => food.IdFood));

            foreach (var response in responses)
                response.SoldQuantity = soldQuantities.GetValueOrDefault(response.IdFood);

            return responses;
        }
    }
}
