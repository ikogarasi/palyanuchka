namespace RestaurantWeb.Services.IServices
{
    public interface IAzureStorageService : IBaseService
    {
        Task<T> UploadAsync<T>(IFormFile file, string folderName);
        Task<T> DeleteAsync<T>(string blobFileName);
    }
}
