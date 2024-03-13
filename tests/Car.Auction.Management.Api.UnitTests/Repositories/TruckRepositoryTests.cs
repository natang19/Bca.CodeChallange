using Car.Auction.Management.Api.Core.CustomExceptions;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Repositories;
using FluentAssertions;

namespace Car.Auction.Management.Api.UnitTests.Repositories;

public class TruckRepositoryTests
{
    private static TruckRepository GetInstance()
    {
        return new TruckRepository();
    }

    [Theory, AutoNSubstituteData]
    public async Task GetById_GivenId_WhenExistTruckOfTheId_ThenReturnsTruck(Truck truck)
    {
        //Arrange
        var repository = GetInstance();
        await repository.Add(truck);
        
        //Act
        var result = await repository.GetById(truck.Id);
        
        //Assert
        result.Should().NotBeNull();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task GetById_GivenId_WhenNotExistTruckOfTheId_ThenReturnsNull(Truck truck)
    {
        //Arrange
        var repository = GetInstance();
        
        //Act
        var result = await repository.GetById(truck.Id);
        
        //Assert
        result.Should().BeNull();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task GetAll_WhenHasTrucksStored_ThenReturnsCollectionOfTrucks(Truck truck)
    {
        //Arrange
        var repository = GetInstance();
        await repository.Add(truck);
        
        //Act
        var result = (await repository.GetAll()).ToList();
        
        //Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
    }
    
    [Theory, AutoNSubstituteData]
    public async Task Add_GivenTruck_WhenIsNotRegistered_ThenRegisterSuccessfully(Truck truck)
    {
        //Arrange
        var repository = GetInstance();
        
        //Act - Assert
        await repository.Invoking(async s => await s.Add(truck)).Should()
            .NotThrowAsync();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task Add_GivenTruck_WhenIsAlreadyRegistered_ThenThrowAnException(Truck truck)
    {
        //Arrange
        var repository = GetInstance();
        await repository.Add(truck);
        
        //Act - Assert
        await repository.Invoking(async s => await s.Add(truck)).Should()
            .ThrowAsync<DuplicatedIdException>();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task Update_WhenIsValidUpdateOperation_ThenUpdateSuccessfully(Truck truck)
    {
        //Arrange
        var repository = GetInstance();
        await repository.Add(truck);
        truck.EnableAuction();
        
        //Act - Assert
        await repository.Invoking(async s => await s.Update(truck)).Should()
            .NotThrowAsync();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task Update_WhenIsInvalidUpdateOperation_ThenThrowAnException(Truck truck)
    {
        //Arrange
        var repository = GetInstance();
        truck.EnableAuction();
        
        //Act - Assert
        await repository.Invoking(async s => await s.Update(truck)).Should()
            .ThrowAsync<InvalidOperationException>();
    }
}