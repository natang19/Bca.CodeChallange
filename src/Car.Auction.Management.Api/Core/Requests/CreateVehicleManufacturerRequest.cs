using FluentValidation;

namespace Car.Auction.Management.Api.Core.Requests;

public record CreateVehicleManufacturerRequest(string Name);

public class RegisterVehicleManufacturerRequestValidator : AbstractValidator<CreateVehicleManufacturerRequest> 
{
    public RegisterVehicleManufacturerRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(ErrorMessageTemplate.RequiredPropertyNotSet);
    }
}