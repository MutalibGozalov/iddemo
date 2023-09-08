using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

    public async Task<IActionResult> CallApi()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var client = new HttpClient();
        client.SetBearerToken(accessToken);
        // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var content = await client.GetAsync("http://localhost:5001/identity");
        ViewBag.Json = await content.Content.ReadAsStringAsync();
        return View("json");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
