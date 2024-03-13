using Car.Auction.Management.Api.Core.Requests;
using FluentAssertions;
using FluentValidation;

namespace Car.Auction.Management.Api.UnitTests.ValidatorsTests;

public class CreateHatchbackRequestValidatorTests
{
    [Fact]
    public void Validate_WhenRequestIsInvalid_ThenThrowsException()
    {
        //Arrange
        var validator = new CreateHatchbackRequestValidator();
        var registerHatchbackRequest = new CreateHatchbackRequest();

        //Act - Assert
        validator.Invoking(s => s.ValidateAndThrow(registerHatchbackRequest))
            .Should()
            .Throw<ValidationException>();
    }
}