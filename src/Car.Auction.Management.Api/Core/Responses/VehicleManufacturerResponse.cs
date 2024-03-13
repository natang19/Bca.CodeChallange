namespace Car.Auction.Management.Api.Core.Responses;

public record VehicleManufacturerResponse
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
}