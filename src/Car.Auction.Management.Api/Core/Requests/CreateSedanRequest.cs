using FluentValidation;

namespace Car.Auction.Management.Api.Core.Requests;

public record CreateSedanRequest : CreateVehicleRequest;

public class CreateSedanRequestValidator : CreateVehicleRequestValidator
{
    public CreateSedanRequestValidator()
    {
        RuleFor(x => x.QuantityDoors)
            .LessThanOrEqualTo((short)4).WithMessage(ErrorMessageTemplate.QuantityDoorsOverflow);
    }
}