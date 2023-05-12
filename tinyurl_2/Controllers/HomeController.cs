using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using tinyurl_2.Models;

namespace tinyurl_2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IDBClient dbClient;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        dbClient = new CosmosDBClient();
    }

    public IActionResult Index()
    {
        return View();
    }

    async public Task<IActionResult> redirect(string id)
    {
        KeyValue response = await this.dbClient.get(id);
        if (string.IsNullOrEmpty(response.url))
        {
            return View(new RedirectURLViewModel
            {
                url = "NotFound"
            });
        }
        return new RedirectResult(response.url, true);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult url()
    {
        return View();
    }

    async public Task<IActionResult> getTinyUrl(string url)
    {
        try
        {
            URLHelper.validate(url);

        } catch(Exception e) {

            return View(new TinyURLViewModel
            {
                url = e.Message
            }) ;
        }

        string code = URLHelper.Shorten(url);
        KeyValue checkExists = await this.dbClient.get(code);

        if(string.IsNullOrEmpty(checkExists.url))
        {
            
            KeyValue result = await this.dbClient.put(url, code);

        }

        string finalURL = URLHelper.baseURL + code;

        return View(new TinyURLViewModel
        {
            url = finalURL
        }) ;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string error=null)
    {
        if (string.IsNullOrEmpty(error))
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        } else {
            return View(new ErrorViewModel { RequestId = error});
        }
    }
}

