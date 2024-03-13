namespace Car.Auction.Management.Api.Core.Models;

public abstract class BaseEntity
{
    public Guid Id { get; }
    public DateTime CreationDate { get; private set; }

    protected BaseEntity(Guid id)
    {
        Id = id;
        CreationDate = DateTime.Now;
    }
}