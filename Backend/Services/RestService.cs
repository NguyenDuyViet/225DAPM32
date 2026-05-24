using AutoMapper;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
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

        public async Task<PagedResult<Restaurant>> GetPagedRestaurantsAsync(int page, int pageSize, int? categoryId = null, string? search = null, string? district = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            int totalItems = await _restRepository.CountAsync(categoryId, search, district, minPrice, maxPrice);
            List<Restaurant> items = await _restRepository.GetPagedAsync(page, pageSize, categoryId, search, district, minPrice, maxPrice);

            return new PagedResult<Restaurant>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
        {
            var data = await _restRepository.GetByIdAsync(id);
            return data;
        }

        public async Task<Restaurant> CreateRestaurantAsync(RestRequestDTO rest)
        {
            var restaurants = await _restRepository.GetAllAsync();
            var restaurant = _mapper.Map<Restaurant>(rest);
            restaurant.IdRestaurant = restaurants.Any()
                ? restaurants.Max(r => r.IdRestaurant) + 1
                : 1;

            await _restRepository.AddAsync(restaurant);
            await _restRepository.SaveChangesAsync();

            return restaurant;
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
