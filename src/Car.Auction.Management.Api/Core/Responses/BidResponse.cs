namespace Car.Auction.Management.Api.Core.Responses;

public record BidResponse(DateOnly CreationDate, TimeOnly CreationTime, decimal Value, Guid Id);