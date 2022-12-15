using RestaurantWeb.Models;
using RestaurantWeb.Services.IServices;

namespace RestaurantWeb.Services
{
    public class AzureStorageService : BaseService, IAzureStorageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public string ContainerUrlString { get; private set; }

        public AzureStorageService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
            : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            ContainerUrlString = configuration.GetValue<string>("BlobContainerUrl");
        }

        public async Task<T> DeleteAsync<T>(string blobFileName)
        {
            return await this.SendAsync<T>(new ApiRequest(){
                ApiType = SD.ApiType.DELETE,
                Url = SD.AzureBlobAPIBase + "/api/AzureStorage/filename?filename=" + blobFileName.Replace("/","%2F"),
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
