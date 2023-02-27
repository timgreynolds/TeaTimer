using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace TeaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeasController : ControllerBase
{
    private readonly ILogger<TeasController> _logger;

    public TeasController(ILogger<TeasController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    public IActionResult Teas()
    {
        _logger.LogTrace("Get Teas Action");
        return new OkObjectResult(new[] { new { Name = "Earl Grey", SteepTime = TimeSpan.FromMinutes(2), BrewTemp = 212 },
                                          new { Name = "Lady Grey", SteepTime = TimeSpan.FromSeconds(90), BrewTemp = 185 }});
    }

    [HttpGet("{Id}"), ActionName("GetById")]
    public IActionResult Teas(string Id)
    {
        _logger.LogTrace("Get Tea by Id Action");
        return new OkObjectResult(new[] { new { Id = 1, Name = "Earl Grey", SteepTime = TimeSpan.FromMinutes(2), BrewTemp = 212 } });
    }

    [HttpPost()]
    public IActionResult Teas([FromHeader(Name = "Host")] string host, [FromBody] JsonDocument body)
    {
        int id = 0;
        if (body.RootElement.TryGetProperty("Id", out JsonElement idElement))
        {
            id = idElement.GetInt32();
        }
        _logger.LogTrace($"Post Tea Action {body}");
        return new CreatedResult($"http://{host}/api/teas/{id}", body);
    }
}
