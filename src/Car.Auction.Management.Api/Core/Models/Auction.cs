using Car.Auction.Management.Api.Core.CustomExceptions;

namespace Car.Auction.Management.Api.Core.Models;

public class Auction : BaseEntity
{
    public DateTime LastUpdate { get; private set; }
    public Guid VehicleId { get; private set; }
    public Bid? StartingBid { get; private set; }
    public Bid? HighestBid { get; private set; }
    public bool Active { get; private set; }

    public Auction(Guid vehicleId, Guid id) : base(id)
    {
        VehicleId = vehicleId;
    }

    public void SetStartingBid(Bid bid)
    {
        if (StartingBid is not null)
        {
            throw new VehicleAlreadyInAuctionException(VehicleId);
        }
        
        StartingBid = bid;
        HighestBid = bid;
        LastUpdate = CreationDate;
        Active = true;
    }

    public void AddNewBid(Bid bid)
    {
        if (bid.Value <= HighestBid!.Value)
        {
            throw new BidInsufficientException(Id);
        }
        
        HighestBid = bid;
        LastUpdate = bid.CreationDate;
    }

    public void Finish()
    {
        Active = false;
        LastUpdate = DateTime.Now;
    }
}