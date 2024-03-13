using FluentValidation;

namespace Car.Auction.Management.Api.Core.Requests;

public record CreateAuctionBidRequest(decimal BidValue);

public class CreateAuctionBidRequestValidator : AbstractValidator<CreateAuctionBidRequest>
{
    public CreateAuctionBidRequestValidator()
    {
        RuleFor(x => x.BidValue)
            .GreaterThanOrEqualTo(0).WithMessage("Bid value cannot be negative");
    }
}