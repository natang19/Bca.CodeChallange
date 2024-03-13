namespace Car.Auction.Management.Api.Core.Models;

public class Truck(double loadCapacity, short quantityDoors, VehicleModel vehicleModel, short yearManufacture, Guid id) : Vehicle(nameof(Truck), quantityDoors, vehicleModel, yearManufacture, id)
{
    public double LoadCapacity { get; private set; } = loadCapacity;
}