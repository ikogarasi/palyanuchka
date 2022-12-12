namespace Restaurant.Services.AzureBlobStorageAPI.Models
{
    public class BlobResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public BlobDto? Result { get; set; }
        public string DisplayMessage { get; set; } = "";
        public List<string> ErrorMessages { get; set; }
    }
}
