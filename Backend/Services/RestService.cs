using AutoMapper;
using Backend.DTOs.Request;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class RestService
    {
        private readonly RestRepository _restRepository;
        private readonly IMapper _mapper;

        public RestService(RestRepository restRepository, IMapper mapper)
        {
            _restRepository = restRepository;
            _mapper = mapper;
        }

        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            var restaurants = await _restRepository.GetAllAsync();
            return restaurants.ToList();
        }

        public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
        {
            var data = await _restRepository.GetByIdAsync(id);
            return data;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var restaurant = await _restRepository.GetByIdAsync(id);
            if (restaurant == null)
            {
                return false;
            }
            await _restRepository.DeleteAsync(restaurant);
            int result = await _restRepository.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateRestAsync(int id, RestRequestDTO rest)
        {
            var existingRestaurant = await _restRepository.GetByIdAsync(id);
            if (existingRestaurant == null)
            {
                return false;
            }

            _mapper.Map(rest, existingRestaurant);

            _restRepository.Update(existingRestaurant);

            int result = await _restRepository.SaveChangesAsync();
            return result > 0;
        }
    }
}
