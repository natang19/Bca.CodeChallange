using Car.Auction.Management.Api.Core.CustomExceptions;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Repositories;

namespace Car.Auction.Management.Api.Services;

public interface IVehicleService
{
    Task<IEnumerable<Vehicle>> GetAll();
    Task<IEnumerable<T>> GetAll<T>() where T : Vehicle;
    Task<dynamic> GetById(Guid id);
    Task<T> GetById<T>(Guid id) where T : Vehicle;
    Task ReleaseForAuction<T>(T value) where T : Vehicle;
    Task LockForAuction<T>(T value) where T : Vehicle;
}

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository<Hatchback> _hatchbackRepository;
    private readonly IVehicleRepository<Sedan> _sedanRepository;
    private readonly IVehicleRepository<Suv> _suvRepository;
    private readonly IVehicleRepository<Truck> _truckRepository;
    private readonly IServiceProvider _serviceProvider;

    public VehicleService(
        IVehicleRepository<Hatchback> hatchbackRepository, 
        IVehicleRepository<Sedan> sedanRepository, 
        IVehicleRepository<Suv> suvRepository, 
        IVehicleRepository<Truck> truckRepository, 
        IServiceProvider serviceProvider)
    {
        _hatchbackRepository = hatchbackRepository;
        _sedanRepository = sedanRepository;
        _suvRepository = suvRepository;
        _truckRepository = truckRepository;
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<Vehicle>> GetAll()
    {
        var vehicles = new List<Vehicle>();
        
        vehicles.AddRange(await _sedanRepository.GetAll());
        vehicles.AddRange(await _hatchbackRepository.GetAll());
        vehicles.AddRange(await _suvRepository.GetAll());
        vehicles.AddRange(await _truckRepository.GetAll());

        return vehicles;
    }

    public async Task<IEnumerable<T>> GetAll<T>() where T : Vehicle
    {
        var vehicleRepository = _serviceProvider.GetRequiredService<IVehicleRepository<T>>();
        return await vehicleRepository.GetAll();
    }

    public async Task<T> GetById<T>(Guid id) where T : Vehicle
    {
        var vehicleRepository = _serviceProvider.GetRequiredService<IVehicleRepository<T>>();
        var vehicle = await  vehicleRepository.GetById(id);

        if (vehicle is null)
        {
            throw new EntityNotFoundException(nameof(T), id);
        }

        return vehicle;
    }

    public async Task<dynamic> GetById(Guid id)
    {
        var hatchback = await _hatchbackRepository.GetById(id);
        if (hatchback is not null)
        {
            return hatchback;
        }

        var sedan = await _sedanRepository.GetById(id);
        if (sedan is not null)
        {
            return sedan;
        }

        var suv = await _suvRepository.GetById(id);
        if (suv is not null)
        {
            return suv;
        }

        var truck = await _truckRepository.GetById(id);
        if (truck is not null)
        {
            return truck;
        }

        throw new EntityNotFoundException(nameof(Vehicle), id);
    }

    public async Task ReleaseForAuction<T>(T value) where T : Vehicle
    {
        value.DisableAuction();
        var vehicleRepository = _serviceProvider.GetRequiredService<IVehicleRepository<T>>();
        await vehicleRepository.Update(value);
    }
    
    public async Task LockForAuction<T>(T value) where T : Vehicle
    {
        value.EnableAuction();
        var vehicleRepository = _serviceProvider.GetRequiredService<IVehicleRepository<T>>();
        await vehicleRepository.Update(value);
    }
}