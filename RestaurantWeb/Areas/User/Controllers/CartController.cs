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
        private readonly IProductService _productService;

        public CartController(IShoppingCartService shoppingCartService, 
            IProductService productService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
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
                foreach(var each in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (each.Count * each.Product.Price);
                }
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
