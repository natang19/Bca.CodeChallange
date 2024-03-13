using Car.Auction.Management.Api.Core.Requests;

namespace Car.Auction.Management.Api.Core.CustomExceptions;

public class StartBidValueNegativeException() : EntityCustomValidationException(nameof(CreateAuctionRequest), nameof(CreateAuctionRequest.StartBidValue), "Start bid value is negative");