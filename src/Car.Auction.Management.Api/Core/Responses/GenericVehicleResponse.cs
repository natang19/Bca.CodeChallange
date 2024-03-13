namespace Car.Auction.Management.Api.Core.Responses;

public record GenericVehicleResponse : VehicleResponse
{
    public string VehicleType { get; init; } = string.Empty;
}