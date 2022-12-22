using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Services.AzureBlobStorageAPI.Models;
using Restaurant.Services.AzureBlobStorageAPI.Repository.IRepository;

namespace Restaurant.Services.AzureBlobStorageAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AzureStorageController : Controller
    {
        private readonly IAzureStorageRepository _storage;

        public AzureStorageController(IAzureStorageRepository storage)
        {
            _storage = storage;
        }

        [HttpPost(nameof(Upload))]
        public async Task<object> Upload(IFormFile file, string folderName = "")
        {
            return await _storage.UploadAsync(file, folderName);            
        }

        [HttpDelete("filename")]
        public async Task<object> Delete(string filename)
        {
            return await _storage.DeleteAsync(filename);
        }
    }
}
