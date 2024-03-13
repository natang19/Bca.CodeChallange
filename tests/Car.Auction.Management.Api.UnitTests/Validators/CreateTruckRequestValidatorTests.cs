using Car.Auction.Management.Api.Core.Requests;
using FluentAssertions;
using FluentValidation;

namespace Car.Auction.Management.Api.UnitTests.ValidatorsTests;

public class CreateTruckRequestValidatorTests
{
    [Fact]
    public void Validate_WhenRequestIsInvalid_ThenThrowsException()
    {
        //Arrange
        var validator = new CreateTruckRequestValidator();
        var createTruckRequest = new CreateTruckRequest(0);

        //Act - Assert
        validator.Invoking(s => s.ValidateAndThrow(createTruckRequest))
            .Should()
            .Throw<ValidationException>();
    }
}