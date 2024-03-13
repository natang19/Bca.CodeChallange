using Car.Auction.Management.Api.Core.Requests;
using FluentAssertions;
using FluentValidation;

namespace Car.Auction.Management.Api.UnitTests.ValidatorsTests;

public class CreateSuvRequestValidatorTests
{
    [Fact]
    public void Validate_WhenRequestIsInvalid_ThenThrowsException()
    {
        //Arrange
        var validator = new CreateSuvRequestValidator();
        var registerSuvRequest = new CreateSuvRequest(2);

        //Act - Assert
        validator.Invoking(s => s.ValidateAndThrow(registerSuvRequest))
            .Should()
            .Throw<ValidationException>();
    }
}