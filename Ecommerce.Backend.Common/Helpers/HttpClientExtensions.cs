using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Backend.Common.Helpers
{
  public static class HttpClientExtensions
  {
    public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
    {
      var dataAsString = JsonSerializer.Serialize(data);
      var content = new StringContent(dataAsString);
      content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      return httpClient.PostAsync(url, content);
    }

    public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
    {
      var dataAsString = JsonSerializer.Serialize(data);
      var content = new StringContent(dataAsString);
      content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      return httpClient.PutAsync(url, content);
    }

    public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
    {
      var dataAsString = await content.ReadAsStringAsync();
      var data = JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      });
      return data;
    }
  }
}