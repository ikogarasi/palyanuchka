namespace Restaurant.Service.OrderAPI.Models.Dto
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public List<string> ErrorMesssages { get; set; }
        public string DisplayMessage { get; set; } = "";
    }
}
