using FluentValidation;

namespace Car.Auction.Management.Api.Core.Requests;

public record CreateAuctionRequest(Guid VehicleId, decimal StartBidValue);

public class CreateAuctionRequestValidator : AbstractValidator<CreateAuctionRequest>
{
    public CreateAuctionRequestValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessageTemplate.RequiredPropertyNotSet)
            .NotEqual(default(Guid)).WithMessage(ErrorMessageTemplate.RequiredPropertyNotSet);
        
        RuleFor(x => x.StartBidValue)
            .GreaterThanOrEqualTo(0).WithMessage("Cannot be negative")
            .NotEqual(0).WithMessage("Cannot be zero");
    }
}