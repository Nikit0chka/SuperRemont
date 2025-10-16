using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SuperRemont.Controllers.Api.Requests;

[ApiController]
[Route("api/[controller]")]
public class RequestController:Controller
{
    private const string BotToken = "8499124710:AAETNeIzruS8EFmEm_aPH4t_lxt3MlsuOvc";
    private const string TelegramUrl = $"https://api.telegram.org/bot{BotToken}/sendMessage";
    private const string ChatId = "888502263";

    [HttpPost]
    public async Task<IActionResult> Create(CreateRequest request)
    {
        using var httpClient = new HttpClient();

        var payload = new
                      {
                          chat_id = ChatId,
                          text = $"Новая заявка на обратный звонок:\n" +
                                 $"Имя - {request.Name}\n" +
                                 $"Телефон - {request.Phone}",
                      };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(TelegramUrl, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        return Ok(responseContent);
    }

    // // Добавьте этот метод для получения chat_id
    // [HttpGet("getupdates")]
    // public async Task<IActionResult> GetUpdates()
    // {
    //     using var httpClient = new HttpClient();
    //     string url = $"https://api.telegram.org/bot{BotToken}/getUpdates";
    //
    //     var response = await httpClient.GetAsync(url);
    //     var content = await response.Content.ReadAsStringAsync();
    //
    //     return Ok(content);
    // }
}

public readonly record struct CreateRequest(string Name, string Phone);