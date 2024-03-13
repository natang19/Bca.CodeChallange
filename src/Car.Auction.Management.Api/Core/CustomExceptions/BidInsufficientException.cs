namespace Car.Auction.Management.Api.Core.CustomExceptions;

public class BidInsufficientException(Guid auctionId) : Exception($"Auction {auctionId}: Insufficient to cover the actual highest bid");