using Onoicrm.Domain.Models;

namespace Onoicrm.Domain.Services;

public class WappiService
{
    public async Task SendMessageAsync(WhatsAppMessage model)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://wappi.pro/api/async/message/send?profile_id={model.ProfileId}");
        request.Headers.Add("Authorization", model.Token);
        var content = new StringContent("{\r\n    \"body\":" + $"\"{model.Text}\""  +  ",\r\n    \"recipient\": "+  $"\"{model.To}\" "  +  "  \r\n}", null, "text/plain");
        request.Content = content;
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Ошибка при отправки сообшения на номер {model.To}");
        }
    }
}

