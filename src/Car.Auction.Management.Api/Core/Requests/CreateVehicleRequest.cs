using FluentValidation;

namespace Car.Auction.Management.Api.Core.Requests;

public record CreateVehicleRequest
{
    public short QuantityDoors { get; init; }
    public short YearManufacture { get; init; }
    public Guid ManufacturerId { get; init; } = Guid.Empty;
    public string ModelName { get; init; } = string.Empty;
}

public class CreateVehicleRequestValidator : AbstractValidator<CreateVehicleRequest>
{
    public CreateVehicleRequestValidator()
    {
        RuleFor(x => x.QuantityDoors)
            .GreaterThanOrEqualTo((short)2).WithMessage("Cannot have less than 2");
        
        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage(ErrorMessageTemplate.RequiredPropertyNotSet);

        RuleFor(x => x.ModelName)
            .NotEmpty().WithMessage(ErrorMessageTemplate.RequiredPropertyNotSet);
        
        RuleFor(x => x.YearManufacture)
            .GreaterThanOrEqualTo((short)1886).WithMessage("Is less than 1886")
            .LessThanOrEqualTo((short)DateTime.Now.Year).WithMessage("Cannot be in the feature");
    }
}