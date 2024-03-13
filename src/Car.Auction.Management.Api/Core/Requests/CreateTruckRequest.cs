using FluentValidation;

namespace Car.Auction.Management.Api.Core.Requests;

public record CreateTruckRequest(double LoadCapacity) : CreateVehicleRequest;

public class CreateTruckRequestValidator : AbstractValidator<CreateTruckRequest>
{
    public CreateTruckRequestValidator()
    {
        var validator = new CreateVehicleRequestValidator();
        RuleFor(x => x).SetValidator(validator);
        
        RuleFor(x => x.QuantityDoors)
            .LessThanOrEqualTo((short)4).WithMessage(ErrorMessageTemplate.QuantityDoorsOverflow);
        
        RuleFor(x => x.LoadCapacity)
            .GreaterThanOrEqualTo(0).WithMessage("Cannot be less than 0 kg");
    }
}