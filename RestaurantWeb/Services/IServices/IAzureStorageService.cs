namespace RestaurantWeb.Services.IServices
{
    public interface IAzureStorageService : IBaseService
    {
        string ContainerUrlString { get; }

        Task<T> UploadAsync<T>(IFormFile file, string folderName);
        Task<T> DeleteAsync<T>(string blobFileName);
    }
}
