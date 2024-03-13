using Car.Auction.Management.Api.Core.Models;

namespace Car.Auction.Management.Api.Repositories;

public interface IBidRepository : IBaseRepository<Bid>
{
    Task<IEnumerable<Bid>> GetBidsByAuctionId(Guid auctionId);
}

public class BidRepository : BaseRepository<Bid>, IBidRepository
{
    public override Task Update(Bid value)
    {
        throw new NotSupportedException("Bid does not allow update operation");
    }

    public Task<IEnumerable<Bid>> GetBidsByAuctionId(Guid auctionId)
    {
        var bids = Context.Values.Where(v => v.AuctionId == auctionId).Distinct();
        return Task.FromResult(bids);
    }
}