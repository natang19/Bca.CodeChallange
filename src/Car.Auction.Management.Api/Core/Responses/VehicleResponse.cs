namespace Car.Auction.Management.Api.Core.Responses;

public record VehicleResponse
{
    public Guid Id { get; init; } = Guid.Empty;
    public byte QuantityDoors { get; init; }
    public ushort YearManufacture { get; init; }
    public string ModelManufacturer { get; init; } = string.Empty;
    public string ModelName { get; init; } = string.Empty;
    public bool InAuction { get; set; }
}