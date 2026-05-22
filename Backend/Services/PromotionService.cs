using AutoMapper;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class PromotionService
    {
        private readonly PromotionRepository _promotionRepository;
        private readonly IMapper _mapper;

        public PromotionService(PromotionRepository promotionRepository, IMapper mapper)
        {
            _promotionRepository = promotionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PromotionResponse>> GetPromotionsByRestaurantAsync(int restaurantId)
        {
            var promotions = await _promotionRepository.FindAsync(p => p.IdRestaurant == restaurantId);
            return _mapper.Map<IEnumerable<PromotionResponse>>(promotions);
        }

        public async Task<PromotionResponse?> GetPromotionByIdAsync(int id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null)
                return null;

            return _mapper.Map<PromotionResponse>(promotion);
        }

        public async Task<PromotionResponse> CreatePromotionAsync(PromotionRequest request)
        {
            var promotion = _mapper.Map<Promotion>(request);
            await _promotionRepository.AddAsync(promotion);
            await _promotionRepository.SaveChangesAsync();

            return _mapper.Map<PromotionResponse>(promotion);
        }

        public async Task<PromotionResponse?> UpdatePromotionAsync(int id, PromotionRequest request)
        {
            var existingPromotion = await _promotionRepository.GetByIdAsync(id);
            if (existingPromotion == null)
                return null;

            _mapper.Map(request, existingPromotion);
            _promotionRepository.Update(existingPromotion);
            await _promotionRepository.SaveChangesAsync();

            return _mapper.Map<PromotionResponse>(existingPromotion);
        }

        public async Task<bool> DeletePromotionAsync(int id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null)
                return false;

            await _promotionRepository.DeleteAsync(promotion);
            return await _promotionRepository.SaveChangesAsync() > 0;
        }
    }
}
