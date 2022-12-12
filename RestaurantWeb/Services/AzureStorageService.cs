using RestaurantWeb.Models;
using RestaurantWeb.Services.IServices;

namespace RestaurantWeb.Services
{
    public class AzureStorageService : BaseService, IAzureStorageService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AzureStorageService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> DeleteAsync<T>(string blobFileName)
        {
            return await this.SendAsync<T>(new ApiRequest(){
                ApiType = SD.ApiType.DELETE,
                Url = SD.AzureBlobAPIBase + "/api/AzureStorage/" + blobFileName,
                AccessToken = ""
            });
        }

        public async Task<T> UploadAsync<T>(IFormFile file, string folderName="")
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = folderName == "" ? 
                    SD.AzureBlobAPIBase + "/api/AzureStorage/Upload" :
                    SD.AzureBlobAPIBase + $"/api/AzureStorage/Upload?folderName={folderName}",
                Data = file,
                AccessToken = ""
            });
        }
    }
}
