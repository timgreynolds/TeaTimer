using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using com.mahonkin.tim.TeaApi.DataModel;
using com.mahonkin.tim.TeaApi.Exceptions;
using com.mahonkin.tim.TeaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace com.mahonkin.tim.TeaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeasController : ControllerBase
{
    private readonly IDataService<TeaModel> _sqlService;

    public TeasController(IDataService<TeaModel> sqlService)
    {
        _sqlService = sqlService;
    }

    [HttpGet(), ActionName("Get")]
    public async Task<IActionResult> Get()
    {
        List<TeaModel> teas = await _sqlService.GetAsync();
        if (teas.Count > 0)
        {
            return new OkObjectResult(teas);
        }
        return new NotFoundObjectResult(teas);
    }

    [HttpGet("{Id}"), ActionName("GetById")]
    public async Task<IActionResult> GetById(string Id)
    {
        try
        {
            TeaModel tea = await _sqlService.FindByIdAsync(Id);
            if (tea is null)
            {
                return new NotFoundObjectResult(new[] { tea });
            }
            return new OkObjectResult(new[] { tea });
        }
        catch (Exception exception)
        {
            return new InternalServerErrorObjectResult(new ErrorResult(HttpStatusCode.InternalServerError, exception.Message));
        }
    }

    [HttpPost(), ActionName("AddTea")]
    public async Task<IActionResult> AddTea([FromHeader(Name = "Host")] string host, [FromBody] JsonDocument body)
    {
        try
        {
            TeaModel tea = body.Deserialize<TeaModel>() ?? new TeaModel();
            TeaModel newTea = await _sqlService.AddAsync(tea);
            return new CreatedResult($"http://{host}/api/teas/{newTea.Id}", newTea);
        }
        catch (Exception exception)
        {
            if (exception.Message.StartsWith("unique constraint fail", StringComparison.OrdinalIgnoreCase))
            {
                return new ConflictObjectResult(new ErrorResult(HttpStatusCode.Conflict, exception.Message));
            }
            return new InternalServerErrorObjectResult(new ErrorResult(HttpStatusCode.InternalServerError, exception.Message));
        }
    }

    [HttpPut(), ActionName("UpdateTea")]
    public async Task<IActionResult> UpdateTea([FromBody] JsonDocument body)
    {
        try
        {
            TeaModel tea = body.Deserialize<TeaModel>() ?? new TeaModel();
            TeaModel updatedTea = await _sqlService.UpdateAsync(tea);
            return new AcceptedResult();
        }
        catch (Exception exception)
        {
            return new InternalServerErrorObjectResult(new ErrorResult(HttpStatusCode.InternalServerError, exception.Message));
        }
    }

    [HttpDelete(), ActionName("DeleteTea")]
    public async Task<IActionResult> DeleteTea([FromBody] JsonDocument body)
    {
        try
        {
            TeaModel tea = body.Deserialize<TeaModel>() ?? new TeaModel();
            if (await _sqlService.DeleteAsync(tea))
            {
                return new OkResult();
            }
            return new InternalServerErrorObjectResult(new ErrorResult(HttpStatusCode.InternalServerError, "Could not delete tea for unknown reasons."));
        }
        catch (Exception exception)
        {
            return new InternalServerErrorObjectResult(new ErrorResult(HttpStatusCode.InternalServerError, exception.Message));
        }
    }
}