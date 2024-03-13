using Car.Auction.Management.Api.Core.Requests;
using FluentAssertions;
using FluentValidation;

namespace Car.Auction.Management.Api.UnitTests.ValidatorsTests;

public class CreateAuctionBidRequestValidatorTests
{
    [Fact]
    public void ValidateAndThrowAsync_WhenBidValueIsNegative_ThenThrowsException()
    {
        //Arrange
        var validator = new CreateAuctionBidRequestValidator();
        var createAuctionBidRequest = new CreateAuctionBidRequest(-1);
        
        //Act - Assert
        validator.Invoking(s => s.ValidateAndThrow(createAuctionBidRequest))
            .Should()
            .Throw<ValidationException>();
    }
}