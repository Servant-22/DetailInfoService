using DetailInfoService;
using DetailInfoService.Services;
using SoapCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSoapCore();
        services.AddSingleton<IVehicleInformationService, VehicleInformationService>();
        services.AddHttpClient<VehicleInformationService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.UseSoapEndpoint<IVehicleInformationService>("/VehicleInformationService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
        });
    }
}
