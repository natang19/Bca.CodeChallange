namespace Car.Auction.Management.Api.Core.Models;

public class Hatchback(short quantityDoors, VehicleModel vehicleModel, short yearManufacture, Guid id) : Vehicle(nameof(Hatchback), quantityDoors, vehicleModel, yearManufacture, id);