using FluentValidation;

namespace Car.Auction.Management.Api.Core.Requests;

public record CreateHatchbackRequest : CreateVehicleRequest;

public class CreateHatchbackRequestValidator : CreateVehicleRequestValidator
{
    public CreateHatchbackRequestValidator()
    {
        RuleFor(x => x.QuantityDoors)
            .LessThanOrEqualTo((short)4).WithMessage(ErrorMessageTemplate.QuantityDoorsOverflow);
    }
}