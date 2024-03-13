using AutoFixture.Xunit2;
using AutoMapper;
using Car.Auction.Management.Api.Commands.V1;
using Car.Auction.Management.Api.Core;
using Car.Auction.Management.Api.Core.CustomExceptions;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Core.Requests;
using Car.Auction.Management.Api.Repositories;
using Car.Auction.Management.Api.Services;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Car.Auction.Management.Api.UnitTests.Commands.V1;

public class AuctionCommandsTests
{
    private readonly IBaseRepository<Core.Models.Auction> _auctionRepository =  Substitute.For<IBaseRepository<Core.Models.Auction>>();
    private readonly IBidRepository _bidRepository = Substitute.For<IBidRepository>();
    private readonly IVehicleService _vehicleService = Substitute.For<IVehicleService>();
    
    private AuctionCommands GetInstance()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAutoMapper(typeof(AutoMapperProfile));
        serviceCollection.AddScoped<IValidator<CreateAuctionRequest>, CreateAuctionRequestValidator>();
        serviceCollection.AddScoped<IValidator<CreateAuctionBidRequest>, CreateAuctionBidRequestValidator>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var mapper = serviceProvider.GetRequiredService<IMapper>();
        return new AuctionCommands(_auctionRepository, _bidRepository, mapper, serviceProvider, _vehicleService);
    }
    
    [Fact]
    public async Task Create_GivenCreateAuctionRequest_WhenRequestIsInvalid_ThenThrowsException()
    {
        //Arrange
        var createAuctionRequest = new CreateAuctionRequest(Guid.Empty, 0);
        var command = GetInstance();
        
        //Act - Assert
        await command.Invoking(async s => await s.Create(createAuctionRequest))
            .Should()
            .ThrowAsync<ValidationException>();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task Create_GivenVehicleId_WhenVehicleIsAlreadyInAuction_ThenThrowsException(
        CreateAuctionRequest createAuctionRequest,
        Vehicle vehicle)
    {
        //Arrange
        vehicle.EnableAuction();
        _vehicleService.GetById(createAuctionRequest.VehicleId).Returns(vehicle);
        var command = GetInstance();
        
        //Act - Assert
        await command.Invoking(async s => await s.Create(createAuctionRequest))
            .Should()
            .ThrowAsync<VehicleAlreadyInAuctionException>();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task Create_GivenVehicleId_WhenVehicleIsAvailableForAuction_ThenCreateAuction(
        CreateAuctionRequest createAuctionRequest,
        Sedan sedan)
    {
        //Arrange
        sedan.DisableAuction();
        _vehicleService.GetById(createAuctionRequest.VehicleId).Returns(sedan);
        _auctionRepository.Add(Arg.Any<Core.Models.Auction>()).Returns(Task.CompletedTask);
        _bidRepository.Add(Arg.Any<Bid>()).Returns(Task.CompletedTask);
        var command = GetInstance();
        
        //Act
        var result = await command.Create(createAuctionRequest);

        //Assert
        result.Should().Subject.Should().BeOfType<CreatedAtRouteResult>();
        await _vehicleService.Received(1).GetById(Arg.Any<Guid>());
        await _vehicleService.Received(1).LockForAuction(Arg.Any<Sedan>());
        await _auctionRepository.Received(1).Add(Arg.Any<Core.Models.Auction>());
        await _bidRepository.Received(1).Add(Arg.Any<Bid>());
    }
    
    [Theory, AutoNSubstituteData]
    public async Task CreateBid_GivenCreateAuctionBidRequest_WhenRequestIsInvalid_ThenThrowsException(Guid auctionId)
    {
        //Arrange
        var createAuctionBidRequest = new CreateAuctionBidRequest(-5);
        var command = GetInstance();
        
        //Act - Assert
        await command.Invoking(async s => await s.CreateBid(auctionId, createAuctionBidRequest))
            .Should()
            .ThrowAsync<ValidationException>();
    }
        
    [Theory, AutoNSubstituteData]
    public async Task CreateBid_GivenAuctionId_WhenNotExistsAuctionOfId_ThenThrowsException(
        CreateAuctionBidRequest createAuctionBidRequest,
        Guid auctionId)
    {
        //Arrange
        Core.Models.Auction? auction = null;
        _auctionRepository.GetById(auctionId).Returns(auction);
        _bidRepository.Add(Arg.Any<Bid>()).Returns(Task.CompletedTask);
        var command = GetInstance();
        
        //Act - Assert
        await command.Invoking(async s => await s.CreateBid(auctionId, createAuctionBidRequest))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task CreateBid_GivenAuctionId_WhenAuctionOfIdIsAlreadyClose_ThenThrowsException(
        CreateAuctionBidRequest createAuctionBidRequest,
        Core.Models.Auction auction)
    {
        //Arrange
        auction.Finish();
        _auctionRepository.GetById(auction.Id).Returns(auction);
        _bidRepository.Add(Arg.Any<Bid>()).Returns(Task.CompletedTask);
        var command = GetInstance();
        
        //Act - Assert
        await command.Invoking(async s => await s.CreateBid(auction.Id, createAuctionBidRequest))
            .Should()
            .ThrowAsync<AuctionClosedException>();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task CreateBid_GivenAuctionId_WhenBidValueIsLessThanHighestBidValue_ThenThrowsException(Core.Models.Auction auction)
    {
        //Arrange
        auction.SetStartingBid(new Bid(500, auction.Id, Guid.NewGuid()));
        _auctionRepository.GetById(auction.Id).Returns(auction);
        _bidRepository.Add(Arg.Any<Bid>()).Returns(Task.CompletedTask);
        var createAuctionBidRequest = new CreateAuctionBidRequest(100);
        var command = GetInstance();
        
        //Act - Assert
        await command.Invoking(async s => await s.CreateBid(auction.Id, createAuctionBidRequest))
            .Should()
            .ThrowAsync<BidInsufficientException>();
        
        _auctionRepository.Received(1);
        _bidRepository.Received(1);
    }
    
    [Theory, AutoNSubstituteData]
    public async Task CreateBid_GivenAuctionId_WhenAuctionExistsAndIsOpenToReceiveBids_ThenCreateBid_AndRegisterInAuction_AndUpdateAuction(Core.Models.Auction auction)
    {
        //Arrange
        auction.SetStartingBid(new Bid(500, auction.Id, Guid.NewGuid()));
        
        _auctionRepository.GetById(auction.Id).Returns(auction);
        _bidRepository.Add(Arg.Any<Bid>()).Returns(Task.CompletedTask);
        
        var createAuctionBidRequest = new CreateAuctionBidRequest(1000);
        
        var command = GetInstance();
        
        //Act
        var result = await command.CreateBid(auction.Id, createAuctionBidRequest);

        //Assert
        result.Should().Subject.Should().BeOfType<AcceptedAtRouteResult>();
        await _auctionRepository.Received(1).GetById(Arg.Any<Guid>());
        await _auctionRepository.Received(1).Update(auction);
        await _bidRepository.Received(1).Add(Arg.Any<Bid>());
    }
    
    [Theory, AutoNSubstituteData]
    public async Task FinishAuction_GivenAuctionId_WhenAuctionOfIdNotExists_ThenThrowsException(Guid auctionId)
    {
        //Arrange
        var command = GetInstance();
        
        //Act
        await command.Invoking(async s => await s.FinishAuction(auctionId))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }
    
    [Theory, AutoNSubstituteData]
    public async Task FinishAuction_GivenAuctionId_WhenAuctionOfIdExists_AndAlreadyFinished_ThenDoNothing(Core.Models.Auction auction)
    {
        //Arrange
        auction.Finish();
        _auctionRepository.GetById(auction.Id).Returns(auction);
        var command = GetInstance();
        
        //Act
        var result = await command.FinishAuction(auction.Id);
        
        //Act
        result.Should().Subject.Should().BeOfType<NoContentResult>();
        await _auctionRepository.Received(1).GetById(auction.Id);
        await _auctionRepository.Received(0).Update(auction);
        await _vehicleService.Received(0).ReleaseForAuction(Arg.Any<Vehicle>());
        await _vehicleService.Received(0).GetById(Arg.Any<Guid>());
    }
    
    [Theory, AutoNSubstituteData]
    public async Task FinishAuction_GivenAuctionId_WhenAuctionOfIdExists_AndNotFinished_ThenFinishAuction(Truck truck)
    {
        //Arrange
        truck.EnableAuction();
        _vehicleService.ReleaseForAuction(truck).Returns(Task.CompletedTask);
        _vehicleService.GetById(truck.Id).Returns(truck);
        
        var auction = new Core.Models.Auction(truck.Id, Guid.NewGuid());
        var bid = new Bid(1000, auction.Id, Guid.NewGuid());
        auction.SetStartingBid(bid);
        _auctionRepository.GetById(auction.Id).Returns(auction);
        _auctionRepository.Update(auction).Returns(Task.CompletedTask);
        
        var command = GetInstance();
        
        //Act
        var result = await command.FinishAuction(auction.Id);
        
        //Act
        result.Should().Subject.Should().BeOfType<NoContentResult>();
        await _auctionRepository.Received(1).GetById(auction.Id);
        await _auctionRepository.Received(1).Update(auction);
        await _vehicleService.Received(1).ReleaseForAuction(truck);
        await _vehicleService.Received(1).GetById(truck.Id);
    }
}