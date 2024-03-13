namespace Car.Auction.Management.Api.Core.Responses;

public record SuvResponse : VehicleResponse
{
    public byte QuantitySeats { get; init; }
}