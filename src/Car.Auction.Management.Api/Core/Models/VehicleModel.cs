namespace Car.Auction.Management.Api.Core.Models;

public class VehicleModel(VehicleManufacturer manufacturer, string name)
{
    public string Name { get; private set; } = name;
    public VehicleManufacturer Manufacturer { get; private set; } = manufacturer;
}