using FluentValidation;

namespace Car.Auction.Management.Api.Core.Requests;

public record CreateSuvRequest(short QuantitySeats) : CreateVehicleRequest;

public class CreateSuvRequestValidator : AbstractValidator<CreateSuvRequest>
{
    public CreateSuvRequestValidator()
    {
        RuleFor(x => x.QuantityDoors)
            .LessThanOrEqualTo((short)4).WithMessage(ErrorMessageTemplate.QuantityDoorsOverflow);

        RuleFor(x => x.QuantitySeats)
            .GreaterThanOrEqualTo((short)4).WithMessage("Cannot have less than 4")
            .LessThanOrEqualTo((short)7).WithMessage("Cannot have more than 7");
        
        var validator = new CreateVehicleRequestValidator();
        RuleFor(x => x).SetValidator(validator);
    }
}