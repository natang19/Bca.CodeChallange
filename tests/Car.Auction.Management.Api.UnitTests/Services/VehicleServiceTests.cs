using Car.Auction.Management.Api.Core.CustomExceptions;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Repositories;
using Car.Auction.Management.Api.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Car.Auction.Management.Api.UnitTests.Services;

public class VehicleServiceTests
{
    private readonly IVehicleRepository<Hatchback> _hatchbackRepository = Substitute.For<IVehicleRepository<Hatchback>>();
    private readonly IVehicleRepository<Sedan> _sedanRepository = Substitute.For<IVehicleRepository<Sedan>>();
    private readonly IVehicleRepository<Suv> _suvRepository = Substitute.For<IVehicleRepository<Suv>>();
    private readonly IVehicleRepository<Truck> _truckRepository = Substitute.For<IVehicleRepository<Truck>>();

    private VehicleService GetService()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_hatchbackRepository);
        serviceCollection.AddSingleton(_sedanRepository);
        serviceCollection.AddSingleton(_suvRepository);
        serviceCollection.AddSingleton(_truckRepository);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return new VehicleService(_hatchbackRepository, _sedanRepository, _suvRepository, _truckRepository, serviceProvider);
    }

    [Theory, AutoNSubstituteData]
    public async Task GetById_GivenId_AndValidVehicleType_WhenExistsVehicleOfId_ThenReturnsVehicle(Hatchback hatchback)
    {
        //Arrange
        _hatchbackRepository.GetById(hatchback.Id).Returns(hatchback);

        var hatchbackId = hatchback.Id;

        var service = GetService();

        //Act
        var result = await service.GetById<Hatchback>(hatchbackId);

        //Assert
        result.Should().NotBeNull();
    }

    [Theory, AutoNSubstituteData]
    public async Task GetById_GivenId_AndValidVehicleType_WhenVehicleNotExist_ThenThrowAnException(Guid id)
    {
        //Arrange
        var service = GetService();

        //Act - Assert
        await service.Invoking(async s => await s.GetById<Hatchback>(id)).Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Theory, AutoNSubstituteData]
    public async Task GetById_GivenId_AndVehicleOfIdIsHatchbackType_WhenExists_ThenReturnsVehicle(Hatchback hatchback)
    {
        //Arrange
        _hatchbackRepository.GetById(hatchback.Id).Returns(hatchback);
        var service = GetService();

        //Act
        var vehicle = (Hatchback)await service.GetById(hatchback.Id);

        //Assert
        vehicle.Should().NotBeNull();
    }

    [Theory, AutoNSubstituteData]
    public async Task GetById_GivenId_AndVehicleOfIdIsSedanType_WhenExists_ThenReturnsVehicle(Sedan sedan)
    {
        //Arrange
        _sedanRepository.GetById(sedan.Id).Returns(sedan);
        var service = GetService();

        //Act
        var vehicle = (Sedan)await service.GetById(sedan.Id);

        //Assert
        vehicle.Should().NotBeNull();
    }

    [Theory, AutoNSubstituteData]
    public async Task GetById_GivenId_AndVehicleOfIdIsSuvType_WhenExists_ThenReturnsVehicle(Suv suv)
    {
        //Arrange
        _suvRepository.GetById(suv.Id).Returns(suv);
        var service = GetService();

        //Act
        var vehicle = (Suv)await service.GetById(suv.Id);

        //Assert
        vehicle.Should().NotBeNull();
    }

    [Theory, AutoNSubstituteData]
    public async Task GetById_GivenId_AndVehicleOfIdIsTruckType_WhenExists_ThenReturnsVehicle(Truck truck)
    {
        //Arrange
        _truckRepository.GetById(truck.Id).Returns(truck);
        var service = GetService();

        //Act
        var vehicle = (Truck)await service.GetById(truck.Id);

        //Assert
        vehicle.Should().NotBeNull();
    }

    [Theory, AutoNSubstituteData]
    public async Task GetByAll_GivenVehicleType_ThenReturnsVehicleCollectionOfType(List<Sedan> sedans)
    {
        //Arrange
        _sedanRepository.GetAll().Returns(sedans);
        var service = GetService();

        //Act
        var vehicle = await service.GetAll<Sedan>();

        //Assert
        vehicle.Should().NotBeEmpty();
    }

    [Theory, AutoNSubstituteData]
    public async Task GetByAll_WhenHasVehiclesRegistered_ThenReturnsVehicleCollection(
        Hatchback hatchback,
        Sedan sedan,
        Suv suv,
        Truck truck)
    {
        //Arrange
        _hatchbackRepository.GetAll().Returns(new List<Hatchback> { hatchback });
        _sedanRepository.GetAll().Returns(new List<Sedan> { sedan });
        _suvRepository.GetAll().Returns(new List<Suv> { suv });
        _truckRepository.GetAll().Returns(new List<Truck> { truck });
        var service = GetService();

        //Act
        var vehicle = await service.GetAll();

        //Assert
        vehicle.Should().HaveCount(4);
    }
    
    [Theory, AutoNSubstituteData]
    public async Task ReleaseForAuction_GivenVehicle_ThenSetInAuctionPropertyAsFalse_AndUpdateModelAtRespectiveRepository(
        Hatchback hatchback)
    {
        //Arrange
        hatchback.EnableAuction();
        _hatchbackRepository.Update(hatchback).Returns(Task.CompletedTask);
        var service = GetService();

        //Act
        await service.ReleaseForAuction(hatchback);

        //Assert
        hatchback.InAuction.Should().BeFalse();
        _hatchbackRepository.Received(1);
    }
    
    [Theory, AutoNSubstituteData]
    public async Task LockForAuction_GivenVehicle_ThenSetInAuctionPropertyAsTrue_AndUpdateModelAtRespectiveRepository(Hatchback hatchback)
    {
        //Arrange
        hatchback.DisableAuction();
        _hatchbackRepository.Update(hatchback).Returns(Task.CompletedTask);
        var service = GetService();

        //Act
        await service.LockForAuction(hatchback);

        //Assert
        hatchback.InAuction.Should().BeTrue();
        _hatchbackRepository.Received(1);
    }
}