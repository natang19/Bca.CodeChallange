namespace Car.Auction.Management.Api.Core.Models;

public abstract class Vehicle(string vehicleType, short quantityDoors, VehicleModel vehicleModel, short yearManufacture, Guid id) : BaseEntity(id)
{
    public string VehicleType { get; private set; } = vehicleType;
    public short QuantityDoors { get; private set; } = quantityDoors;
    public VehicleModel VehicleModel { get; private set; } = vehicleModel;
    public short YearManufacture { get; private set; } = yearManufacture;
    public bool InAuction { get; private set; }

    public void EnableAuction()
    {
        InAuction = true;
    }

    public void DisableAuction()
    {
        InAuction = false;
    }
}