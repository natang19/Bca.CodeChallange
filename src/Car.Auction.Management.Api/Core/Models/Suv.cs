namespace Car.Auction.Management.Api.Core.Models;

public class Suv(short quantitySeats, short quantityDoors, VehicleModel vehicleModel, short yearManufacture, Guid id) : Vehicle(nameof(Suv).ToUpper(), quantityDoors, vehicleModel, yearManufacture, id)
{
    public short QuantitySeats { get; private set; } = quantitySeats;
}