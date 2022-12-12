using Restaurant.Services.AzureBlobStorageAPI.Models;

namespace Restaurant.Services.AzureBlobStorageAPI.Repository.IRepository
{
    public interface IAzureStorageRepository
    {
        Task<BlobResponseDto> UploadAsync(IFormFile file, string folderName = "");
        //Task<BlobDto> DownloadAsync(string blobFileName);
        Task<BlobResponseDto> DeleteAsync(string blobFileName);
        //Task<List<BlobDto>> ListAsync();
    }
}
