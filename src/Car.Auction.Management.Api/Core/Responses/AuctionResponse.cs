namespace Car.Auction.Management.Api.Core.Responses;

public record AuctionResponse(string CreationDate, string LastUpdateDate, string LastUpdateTime, Guid VehicleId, decimal HighestBidValue, bool Active, Guid Id);