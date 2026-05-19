using AutoMapper;
using Backend.DTOs.Request;
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
        public async Task<List<Food>> GetAllFoodsAsync()
        {
            var foods = await _foodRepository.GetAllAsync();

            return foods.ToList();
        }

        public async Task<Food> GetFoodByIdAsync(int id)
        {
            var food = await _foodRepository.GetByIdAsync(id);
            return food;
        }

        public async Task<Food> CreateFoodAsync(FoodRequest foodrq)
        {
            var food = _mapper.Map<FoodRequest, Food>(foodrq);

            await _foodRepository.AddAsync(food);
            await _foodRepository.SaveChangesAsync();

            return food;
        }

        public async Task<bool> UpdateFoodAsync(int id, FoodRequest foodrq)
        {
            var food = await _foodRepository.GetByIdAsync(id);
            if (food == null)
                return false;
            _mapper.Map(foodrq, food);
            _foodRepository.Update(food);
            return await _foodRepository.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteFoodAsync(int id)
        {
            var food = await _foodRepository.GetByIdAsync(id);
            if (food == null)
                return false;
            await _foodRepository.DeleteAsync(food);
            return await _foodRepository.SaveChangesAsync() > 0;    
        }
    }
}
