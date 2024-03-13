using System.Collections.Concurrent;
using Car.Auction.Management.Api.Core.CustomExceptions;
using Car.Auction.Management.Api.Core.Models;

namespace Car.Auction.Management.Api.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T?> GetById(Guid id);
    Task Add(T value);
    Task<IEnumerable<T>> GetAll();
    Task Update(T value);
}

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ConcurrentDictionary<Guid, T> Context = new();

    public Task<T?> GetById(Guid id)
    {
        return Task.FromResult(Context.GetValueOrDefault(id));
    }

    public Task Add(T value)
    {
        if (!Context.TryAdd(value.Id, value))
        {
            throw new DuplicatedIdException(nameof(T));
        }
        
        return Task.CompletedTask;
    }

    public Task<IEnumerable<T>> GetAll()
    {
        return Task.FromResult<IEnumerable<T>>(Context.Values.ToList());
    }

    public virtual Task Update(T value)
    {
        var oldValue = Context.GetValueOrDefault(value.Id);
        
        if (!Context.TryUpdate(value.Id, value, oldValue!))
        {
            throw new InvalidOperationException($"{nameof(T)} {value.Id}: Unexpected error when trying to update");
        }

        return Task.CompletedTask;
    }
}