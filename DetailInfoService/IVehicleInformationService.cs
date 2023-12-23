namespace DetailInfoService;

using System.ServiceModel;

[ServiceContract]
public interface IVehicleInformationService
{
    [OperationContract]
    VehicleData GetVehicleData(string vin);
}
