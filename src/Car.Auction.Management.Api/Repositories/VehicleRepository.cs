using Car.Auction.Management.Api.Core.CustomExceptions;
using Car.Auction.Management.Api.Core.Models;

namespace Car.Auction.Management.Api.Repositories;

public interface IVehicleRepository<T> : IBaseRepository<T> where T : Vehicle;

public abstract class VehicleRepository<T> : BaseRepository<T>, IVehicleRepository<T> where T : Vehicle;