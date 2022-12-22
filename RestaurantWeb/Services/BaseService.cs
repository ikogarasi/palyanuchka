using RestaurantWeb.Models;
using RestaurantWeb.Services.IServices;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace RestaurantWeb.Services
{
    public class BaseService : IBaseService
    {
        public BaseService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
            responseModel = new();
        }

        public ResponseDto responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("RestaurantAPI");
                HttpRequestMessage request = new HttpRequestMessage();
                request.Headers.Add("Accept", "application/json");
                request.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null)
                {
                    if (apiRequest.Data is FormFile)
                    {
                        var file = apiRequest.Data as FormFile;
                        var content = new MultipartFormDataContent();
                        content.Add(new StreamContent(file.OpenReadStream()), file.Name, file.FileName);
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = file.Name, FileName = file.FileName
                        };
                        request.Content = content;
                        /*var apiData = apiRequest.Data;
                        request.Content = new MultipartFormDataContent();
                        (request.Content as MultipartFormDataContent)
                            .Add(new StreamContent((apiData as FormFile).OpenReadStream()), 
                                "file", (apiData as FormFile).FileName);*/
                    }
                    else
                        request.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                            Encoding.UTF8, "application/json");
                }

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                }

                HttpResponseMessage apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.GET:
                        request.Method = HttpMethod.Get;
                        break;
                    case SD.ApiType.POST:
                        request.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        request.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        request.Method = HttpMethod.Delete;
                        break;
                }

                apiResponse = await client.SendAsync(request);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDto;
            }
            catch(Exception ex)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
