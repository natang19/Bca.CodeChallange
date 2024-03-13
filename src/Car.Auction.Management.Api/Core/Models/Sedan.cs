namespace Car.Auction.Management.Api.Core.Models;

public class Sedan(short quantityDoors, VehicleModel vehicleModel, short yearManufacture, Guid id)
    : Vehicle(nameof(Sedan), quantityDoors, vehicleModel, yearManufacture, id);