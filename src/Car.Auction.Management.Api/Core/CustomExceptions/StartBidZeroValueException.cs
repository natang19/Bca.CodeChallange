using Car.Auction.Management.Api.Core.Requests;

namespace Car.Auction.Management.Api.Core.CustomExceptions;

public class StartBidZeroValueException() : EntityCustomValidationException(nameof(CreateAuctionRequest), nameof(CreateAuctionRequest.StartBidValue), "Starting bid value is 0");