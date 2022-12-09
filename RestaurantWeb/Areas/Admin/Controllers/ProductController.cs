using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantWeb.Models;
using RestaurantWeb.Services.IServices;

namespace RestaurantWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> list = new();
            var response = await _productService.GetAllProductsAsync<ResponseDto>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

/*        public async Task<List<ProductDto>> GetAll()
        {
            
            
        }*/
    }
}
