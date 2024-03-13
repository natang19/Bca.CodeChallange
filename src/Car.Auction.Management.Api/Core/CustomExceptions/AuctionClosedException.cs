namespace Car.Auction.Management.Api.Core.CustomExceptions;

public class AuctionClosedException(Guid auctionId) : EntityCustomValidationException(nameof(Models.Auction), auctionId, "Closed");