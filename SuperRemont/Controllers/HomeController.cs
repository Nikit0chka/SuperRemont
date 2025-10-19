using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

namespace SuperRemont.Controllers;

public class HomeController:Controller
{
    private static string? _cachedHtml;
    private static DateTime _lastCacheTime = DateTime.MinValue;
    private static bool _browserDownloaded;

    public async Task<IActionResult> Index()
    {
        // Кэшируем на 1 час
        if (_cachedHtml != null && DateTime.Now - _lastCacheTime <= TimeSpan.FromHours(1))
            return Content(_cachedHtml, "text/html");

        _cachedHtml = await PrerenderHtml();
        _lastCacheTime = DateTime.Now;

        return Content(_cachedHtml, "text/html");
    }

    private static async Task<string> PrerenderHtml()
    {
        if (!_browserDownloaded)
        {
            await new BrowserFetcher().DownloadAsync();
            _browserDownloaded = true;
        }

        await using var browser = await Puppeteer.LaunchAsync(new()
                                                              {
                                                                  Headless = true,
                                                                  Args = ["--no-sandbox", "--disable-setuid-sandbox"]
                                                              });

        await using var page = await browser.NewPageAsync();

        var htmlPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
        var originalHtml = await System.IO.File.ReadAllTextAsync(htmlPath);

        await page.SetContentAsync(originalHtml);

        try
        {
            await page.WaitForSelectorAsync("#root",
                                            new()
                                            { Timeout = 2000 });
        }
        catch
        {
            // ignored
        }

        await page.WaitForNetworkIdleAsync(new()
                                           { Timeout = 2000 });

        await Task.Delay(2000);

        var renderedHtml = await page.GetContentAsync();

        return renderedHtml;
    }
}