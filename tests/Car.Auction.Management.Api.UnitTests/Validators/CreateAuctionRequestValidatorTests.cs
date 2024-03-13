using Car.Auction.Management.Api.Core.Requests;
using FluentAssertions;
using FluentValidation;

namespace Car.Auction.Management.Api.UnitTests.ValidatorsTests;

public class CreateAuctionRequestValidatorTests
{
    [Fact]
    public async Task ValidateAndThrowAsync_WhenGuidIsInvalidValue_ThenThrowsException()
    {
        //Arrange
        var validator = new CreateAuctionRequestValidator();
        var createAuctionRequest = new CreateAuctionRequest(Guid.Empty, 0);

        //Act - Assert
        await validator.Invoking(async s => await s.ValidateAndThrowAsync(createAuctionRequest))
            .Should()
            .ThrowAsync<ValidationException>();
    }
}