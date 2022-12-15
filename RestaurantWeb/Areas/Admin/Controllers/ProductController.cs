using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantWeb.Models;
using RestaurantWeb.Services.IServices;
using System.Text;

namespace RestaurantWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        #region Dependecy Injection / Constructor
        private readonly IProductService _productService;
        private readonly IAzureStorageService _azureStorageService;

        public ProductController(IProductService productService, 
            IAzureStorageService azureStorageService)
        {
            _productService = productService;
            _azureStorageService = azureStorageService;
        }

        #endregion

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

        [HttpGet]
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpsertPOST(ProductDto dto, IFormFile file = null)
        {
            ResponseDto uploadResponse = null;
            ResponseDto response = new();

            if (file != null)
            {
                /*string fileName = dto.ProductName.ToLower() + Path.GetExtension(file.FileName);
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);

                    IFormFile renamedFile = new FormFile(ms, 0, ms.Length, dto.ProductName.ToLower(), fileName);
                    renamedFile.Headers.Add("Content-Type", "image/jpeg");
                    renamedFile.Headers.Add("Content-Disposition", $"name=\"file\";filename=\"{fileName}\"");

                    uploadResponse = await _azureStorageService.UploadAsync<ResponseDto>(
                        renamedFile,
                        "Images");
                }*/

                if (dto.ImageURL != null)
                {
                    await _azureStorageService.DeleteAsync<ResponseDto>(
                        dto.ImageURL.Replace(_azureStorageService.ContainerUrlString + "/", ""));
                }

                uploadResponse = await _azureStorageService.UploadAsync<ResponseDto>(file, "Images");
            }

            if (uploadResponse != null && uploadResponse.IsSuccess)
                dto.ImageURL = JsonConvert.DeserializeObject<BlobDto>(uploadResponse.Result.ToString()).Uri;

            if (dto.ProductId == 0)
            {
                response = await _productService.CreateProductAsync<ResponseDto>(dto);
            }
            else
            {
                response = await _productService.UpdateProductAsync<ResponseDto>(dto);
            }

            return RedirectToAction("Index");
        }

        #region API_CALL

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            ResponseDto response = await _productService.GetProductByIdAsync<ResponseDto>(id);
            ProductDto product;

            if (response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
                response = await _azureStorageService.DeleteAsync<ResponseDto>(
                    product.ImageURL.Replace(_azureStorageService.ContainerUrlString+"/", ""));
                response = await _productService.DeleteProductAsync<ResponseDto>(product.ProductId);
                return Json(new { IsSuccess = true });
            }

            return Json(new { IsSucces = false });
        }

        #endregion
    }
}
