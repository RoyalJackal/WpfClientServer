using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Services;

public static class HttpService
{
    private static string? Token = null;
    private const string ApiPath = "https://localhost:7023";
    private static readonly HttpClient Http = new HttpClient();

    public static async Task<HttpResponseMessage> GetAsync(string path, bool needAuth = false)
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiPath + path))
        {
            if (needAuth)
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);
    
            return await Http.SendAsync(requestMessage);
        }
    }
    
    public static async Task<HttpResponseMessage> PostAsync(string path, object dto, bool needAuth = false)
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiPath + path))
        {
            if (needAuth)
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);

            var json = JsonSerializer.Serialize(dto);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
    
            return await Http.SendAsync(requestMessage);
        }
    }

    public static void SetToken(string? token)
    {
        Token = token;
    }

    public static void ClearToken()
    {
        Token = null;
    }
}