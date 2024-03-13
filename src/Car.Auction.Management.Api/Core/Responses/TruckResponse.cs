namespace Car.Auction.Management.Api.Core.Responses;

public record TruckResponse : VehicleResponse
{
    public double LoadCapacity { get; init; } = 0;
}