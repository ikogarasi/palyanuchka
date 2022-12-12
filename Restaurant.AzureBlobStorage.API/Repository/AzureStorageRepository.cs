using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Restaurant.Services.AzureBlobStorageAPI.Models;
using Restaurant.Services.AzureBlobStorageAPI.Repository.IRepository;

namespace Restaurant.Services.AzureBlobStorageAPI.Repository
{
    public class AzureStorageRepository : IAzureStorageRepository
    {
        #region Dependency Injection / Constructor

        private readonly BlobContainerClient _blobContainerClient;

        public AzureStorageRepository(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient;
        }

        #endregion
        public async Task<BlobResponseDto> UploadAsync(IFormFile file, string folderName="")
        {
            BlobResponseDto response = new();

            try
            {
                BlobClient client; 
                if (folderName == "")
                    client = _blobContainerClient.GetBlobClient(file.FileName);
                else
                    client = _blobContainerClient.GetBlobClient($"{folderName}/{file.FileName}");

                await using (Stream data = file.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }

                response.DisplayMessage = $"File {file.FileName} Uploaded";
                response.IsSuccess = true;
                response.Result = new()
                {
                    Uri = client.Uri.AbsoluteUri,
                    Name = client.Name
                };

            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                response.DisplayMessage = $"File with name {file.FileName} already exists in container";
                //_logger.LogError(response.DisplayMessage);
                response.IsSuccess = false;
                return response;
            }
            catch (RequestFailedException ex)
            {
                response.DisplayMessage = $"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}";
                //_logger.LogError(response.DisplayMessage);
                response.IsSuccess = false;
                return response;
            }

            return response;
        }

        public async Task<BlobResponseDto> DeleteAsync(string blobFileName)
        {
            BlobClient file = _blobContainerClient.GetBlobClient(blobFileName);

            try
            {
                await file.DeleteAsync();
            }
            catch(RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                string errorMessage = $"File {blobFileName} was not found.";
                //_logger.LogError(errorMessage);
                return new BlobResponseDto { IsSuccess = false, DisplayMessage = errorMessage };
            }

            return new BlobResponseDto
            {
                IsSuccess = true,
                DisplayMessage = $"File: {blobFileName} has been deleted."
            };
        }

    }
}
