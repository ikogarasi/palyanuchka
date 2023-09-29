using Microsoft.AspNetCore.Mvc;
using Restaurant.MessageBus;
using Restaurant.Service.ShoppingCartAPI.Models.Dto;
using Restaurant.Services.ShoppingCartAPI.Messages;
using Restaurant.Services.ShoppingCartAPI.Models.Dto;
using Restaurant.Services.ShoppingCartAPI.RabbitMQSender;
using Restaurant.Services.ShoppingCartAPI.Repository.IRepository;

namespace Restaurant.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartAPIController : Controller
    {
        protected ResponseDto _responseDto;
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IMessageBus _messageBus;
        private readonly IRabbitMQCartMessageSender _rabbitMQCartMessageSender;

        public CartAPIController(ICartRepository cartRepository, ICouponRepository couponRepository,
            IMessageBus messageBus, IRabbitMQCartMessageSender rabbitMQCartMessageSender)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _rabbitMQCartMessageSender = rabbitMQCartMessageSender;
            _messageBus = messageBus;
            _responseDto = new();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserId(userId);
                _responseDto.Result = cartDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDto;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cart = await _cartRepository.CreateUpdateCart(cartDto);
                _responseDto.Result = cart;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDto;
        }

        [HttpPut("UpdateCart")]
        public async Task<object> UpdateCart(CartDto cartDto)
        {
            try
            {
                CartDto cart = await _cartRepository.CreateUpdateCart(cartDto);
                _responseDto.Result = cart;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDto;
        }

        [HttpDelete("RemoveCart")]
        public async Task<object> RemoveCart(int cartId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveFromCart(cartId);
                _responseDto.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDto;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                _responseDto.Result = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, 
                    cartDto.CartHeader.CouponCode);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                _responseDto.Result = await _cartRepository.RemoveCoupon(userId);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }
        [HttpPost("Checkout")]
        public async Task<object> Checkout([FromBody] CheckoutHeaderDto checkoutHeaderDto)
        {
            try
            {
                CartDto cart = await _cartRepository.GetCartByUserId(checkoutHeaderDto.UserId);
                
                if (cart == null)
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(checkoutHeaderDto.CouponCode))
                {
                    CouponDto coupon = await _couponRepository.GetCoupon(checkoutHeaderDto.CouponCode);
                    
                    if (checkoutHeaderDto.DiscountTotal != coupon.DiscountAmount)
                    {
                        _responseDto.IsSuccess = false;
                        _responseDto.Result = coupon.DiscountAmount;
                        _responseDto.ErrorMessages = new List<string>() { "Coupon Price has been changed, please confirm" };
                        _responseDto.DisplayMessage = "Coupon Price has been changed, please confirm";

                        return _responseDto;
                    }
                }

                checkoutHeaderDto.CartDetails = cart.CartDetails;
                //await _messageBus.PublishMessage(checkoutHeaderDto, "checkoutmessagetopic");

                _rabbitMQCartMessageSender.SendMessage(checkoutHeaderDto, "checkoutqueue");
                await _cartRepository.ClearCart(checkoutHeaderDto.UserId);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDto;
        }
    }
}
