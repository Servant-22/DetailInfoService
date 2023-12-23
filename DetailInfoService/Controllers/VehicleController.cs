using DetailInfoService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DetailInfoService.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleController : ControllerBase
{
    private readonly NhtsaApiClient _nhtsaApiClient;

    public VehicleController(NhtsaApiClient nhtsaApiClient)
    {
        _nhtsaApiClient = nhtsaApiClient;
    }

    [HttpGet("{vin}")]
    public async Task<ActionResult<VehicleData>> Get(string vin)
    {
        try
        {
            var vehicleData = await _nhtsaApiClient.GetVehicleDataAsync(vin);
            if (vehicleData == null)
            {
                return NotFound();
            }
            return vehicleData;
        }
        catch (Exception ex)
        {
            // Log exception details here
            return StatusCode(500, "Internal Server Error");
        }
    }
}