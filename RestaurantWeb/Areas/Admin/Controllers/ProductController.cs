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

        public async Task<IActionResult> Upsert(int id = 0)
        {
            if (id == 0)
                return View(new ProductDto());

            ProductDto productFromDb = new();
            var response = await _productService.GetProductByIdAsync<ResponseDto>(id);
            if (response != null && response.IsSuccess)
            {
                productFromDb = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            return View(productFromDb);            
        }
/*
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductDto dto, IFormFile? file)
        {
            if (file != null)
            {
                string fileName = dto.ProductName + Path.GetExtension(file.FileName);

                if (dto.ImageURL != null)
                {
                    
                }
            }
        }*/
    }
}
