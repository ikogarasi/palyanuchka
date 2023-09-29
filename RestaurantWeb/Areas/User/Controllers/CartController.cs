using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantWeb.Models;
using RestaurantWeb.Services.IServices;

namespace RestaurantWeb.Areas.User.Controllers
{
    [Area("User")]
    public class CartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICouponService _couponService;
        private readonly IProductService _productService;

        public CartController(IShoppingCartService shoppingCartService, 
            IProductService productService,
            ICouponService couponService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
            _couponService = couponService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            cartDto.CartHeader.UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var response = await _shoppingCartService.ApplyCoupon<ResponseDto>(cartDto, accessToken);

            if (response == null && !response.IsSuccess)
                ViewBag.ErrorMessage = "Invalid coupon code";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var response = await _shoppingCartService.RemoveCoupon<ResponseDto>(userId, accessToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var response = await _shoppingCartService.RemoveFromCartAsync<ResponseDto>(cartId, accessToken);

            if (response == null && !response.IsSuccess)
                ViewBag.ErrorMessage = "Cannot remove item from cart";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cart)
        {
            try 
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                ResponseDto response = await _shoppingCartService.Checkout<ResponseDto>(cart.CartHeader, accessToken);
                
                if (!response.IsSuccess)
                {
                    TempData["Error"] = response.DisplayMessage;
                    return RedirectToAction(nameof(Checkout));
                }
                
                return RedirectToAction(nameof(Confirmation));
            }
            catch(Exception ex)
            {
                return View(cart);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var response = await _shoppingCartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);

            CartDto cartDto = new();
            if (response != null && response.IsSuccess)
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));

            if (cartDto.CartHeader != null)
            {
                if(!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon<ResponseDto>(cartDto.CartHeader.CouponCode, accessToken);
                    if (coupon != null && coupon.IsSuccess)
                    {
                        var couponObj = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(coupon.Result));
                        cartDto.CartHeader.DiscountTotal = couponObj.DiscountAmount;
                        cartDto.CartHeader.CouponCode = couponObj.CouponCode;
                    }
                }

                foreach(var each in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (each.Count * each.Product.Price);
                }

                cartDto.CartHeader.OrderTotal -= cartDto.CartHeader.DiscountTotal;
            }

            return cartDto;
        }

        #region API_CALL

        [HttpGet]
        public async Task<IActionResult> GetCartDetails()
        {
            CartDto cartDto = await LoadCartDtoBasedOnLoggedInUser();
            return Json(new { data = cartDto.CartDetails });
        }

        #endregion
    }
}
