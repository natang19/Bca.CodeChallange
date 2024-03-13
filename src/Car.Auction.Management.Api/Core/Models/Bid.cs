namespace Car.Auction.Management.Api.Core.Models;

public class Bid : BaseEntity
{
    public Guid AuctionId { get; private set; } 
    public decimal Value { get; private set; }

    public Bid(decimal value, Guid auctionId, Guid id) : base(id)
    {
        Value = value;
        AuctionId = auctionId;
    }
}