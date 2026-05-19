using AutoMapper;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly CartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;

        public CartService(CartRepository cartRepository, CartItemRepository cartItemRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<bool> ClearCartAsync(int IdCart)
        {
            var cart = await _cartRepository.GetByIdAsync(IdCart);
            if (cart == null)
                return false;
            cart.CartFoods.Clear();
            _cartRepository.Update(cart);
            return await _cartRepository.SaveChangesAsync() > 0;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetByIdAsync(userId);
            return cart;
        }
        //Hàm tăng số lượng món ăn trong giỏ hàng
        public async Task<CartFood> Increase(int IdFood)
        {
            var existingCart = await _cartItemRepository.GetByIdAsync(IdFood);
            if (existingCart == null)
                return null;
            existingCart.Quantity += 1;
            _cartItemRepository.Update(existingCart);
            return existingCart;
        }

        ////Hàm giảm số lượng món ăn trong giỏ hàng
        //public async Task<CartFood> Decrease(int IdCart)
        //{
        //    var existingCart = await _cartItemRepository.GetByIdAsync(IdFood);
        //    if (existingCart == null)
        //        return null;

        //    if (cartItem.Quantity > 1)
        //    {
        //        --cartItem.Quantity;
        //        // giam so luong 
        //    }
        //    else
        //    {
        //        cart.RemoveAll(p => p.ProductId == Id);
        //        // xoa luon san pham do
        //    }
        //    if (cart.Count == 0)
        //    {
        //        HttpContext.Session.Remove("Cart");

        //    }
        //    else
        //    {
        //        HttpContext.Session.SetJson("Cart", cart);
        //        //tao session gio hang moi
        //    }
        //    var existingCart = await _cartItemRepository.GetByIdAsync(IdFood);
        //    if (existingCart == null)
        //        return null;
        //    existingCart.Quantity += 1;
        //    _cartItemRepository.Update(existingCart);
        //    return existingCart;
        //}
        //Xem lại
        public async Task<bool> DeleteCartAsync(int userId)
        {
            var cart = await _cartRepository.GetByIdAsync(userId);
            if (cart == null)
                return false;
            await _cartRepository.DeleteAsync(cart);
            return await _cartRepository.SaveChangesAsync() > 0;
        }
    }
}
