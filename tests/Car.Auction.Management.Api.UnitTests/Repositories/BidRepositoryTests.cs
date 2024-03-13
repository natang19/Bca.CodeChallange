using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Repositories;
using FluentAssertions;

namespace Car.Auction.Management.Api.UnitTests.Repositories;

public class BidRepositoryTests
{
    private static BidRepository GetInstance()
    {
        return new BidRepository();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task Update_WhenMethodCalled_ThenThrowAnException(Bid bid)
    {
        //Arrange
        var repository = GetInstance();
        await repository.Add(bid);
        
        //Act - Assert
        await repository.Invoking(async s => await s.Update(bid)).Should()
            .ThrowAsync<NotSupportedException>();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task GetBidsByAuctionId_GivenAuctionId_WhenHasBidsAssociatesToAuctionId_ThenReturnBidCollectionOfAuctionId(Guid auctionId)
    {
        //Arrange
        var bids = new List<Bid>
        {
            new(10, auctionId, Guid.NewGuid()), 
            new(10, auctionId, Guid.NewGuid()), 
            new(10, Guid.NewGuid(), Guid.NewGuid())
        };
        var repository = GetInstance();

        foreach (var bid in bids)
        {
            await repository.Add(bid);   
        }
        
        //Act
        var result = (await repository.GetBidsByAuctionId(auctionId)).ToList();
        
        //Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(2);
    }
}