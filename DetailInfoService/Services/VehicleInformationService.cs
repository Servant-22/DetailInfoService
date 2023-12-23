using System.ServiceModel;

namespace DetailInfoService.Services;

public class VehicleInformationService : IVehicleInformationService
{
    private readonly HttpClient _httpClient;

    public VehicleInformationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public VehicleData GetVehicleData(string vin)
    {
        if (string.IsNullOrEmpty(vin))
        {
            throw new ArgumentException("VIN cannot be null or empty.");
        }

        try
        {
            var apiResponse = GetVehicleDataFromApi(vin).Result;
            return ConvertToVehicleData(apiResponse);
        }
        catch (Exception ex)
        {
            // Voeg meer gedetailleerde foutafhandeling toe zoals nodig
            throw new FaultException($"An error occurred while processing the request: {ex.Message}");
        }
    }

    private async Task<NhtsaApiResponse> GetVehicleDataFromApi(string vin)
    {
        var response = await _httpClient.GetAsync($"https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<NhtsaApiResponse>();
    }

    private VehicleData ConvertToVehicleData(NhtsaApiResponse apiResponse)
    {
        var vehicle = new VehicleData
        {
            Make = apiResponse.Results.FirstOrDefault(r => r.Variable == "Make")?.Value ?? "Unknown",
            Model = apiResponse.Results.FirstOrDefault(r => r.Variable == "Model")?.Value ?? "Unknown",
            ModelYear = apiResponse.Results.FirstOrDefault(r => r.Variable == "Model Year")?.Value ?? "Unknown",
            BodyClass = apiResponse.Results.FirstOrDefault(r => r.Variable == "Body Class")?.Value ?? "Unknown",
            VehicleType = apiResponse.Results.FirstOrDefault(r => r.Variable == "Vehicle Type")?.Value ?? "Unknown"
        };

        return vehicle;
    }
}

// NHTSA API Response Models
public class NhtsaApiResponse
{
    public List<NhtsaApiResult> Results { get; set; }
}

public class NhtsaApiResult
{
    public string Value { get; set; }
    public string Variable { get; set; }
}
