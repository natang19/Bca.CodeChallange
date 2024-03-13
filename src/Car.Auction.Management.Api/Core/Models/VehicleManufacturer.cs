namespace Car.Auction.Management.Api.Core.Models;

public class VehicleManufacturer(string name, Guid id) : BaseEntity(id)
{
    public string Name { get; private set; } = name;
}