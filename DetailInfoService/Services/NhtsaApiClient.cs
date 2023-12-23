using System.Net.Http.Json;


namespace DetailInfoService.Services;

public class NhtsaApiClient
{
    private readonly HttpClient _httpClient;

    public NhtsaApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<VehicleData> GetVehicleDataAsync(string vin)
    {
        var response = await _httpClient.GetAsync($"https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json");
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadAsAsync<NhtsaResponse>();
        var results = data.Results;

        return new VehicleData
        {
            Make = results.FirstOrDefault(r => r.Variable == "Make")?.Value,
            Model = results.FirstOrDefault(r => r.Variable == "Model")?.Value,
            ModelYear = results.FirstOrDefault(r => r.Variable == "Model Year")?.Value,
            BodyClass = results.FirstOrDefault(r => r.Variable == "Body Class")?.Value,
            VehicleType = results.FirstOrDefault(r => r.Variable == "Vehicle Type")?.Value
        };
    }
}
public class NhtsaResponse
{
    public List<NhtsaResult> Results { get; set; }
}

public class NhtsaResult
{
    public string Value { get; set; }
    public string Variable { get; set; }
}

