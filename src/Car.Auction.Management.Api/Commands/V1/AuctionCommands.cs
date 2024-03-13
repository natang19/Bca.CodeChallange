using System.Net.Mime;
using Asp.Versioning;
using AutoMapper;
using Car.Auction.Management.Api.Core.CustomExceptions;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Core.Requests;
using Car.Auction.Management.Api.Core.Responses;
using Car.Auction.Management.Api.Queries.V1;
using Car.Auction.Management.Api.Repositories;
using Car.Auction.Management.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Car.Auction.Management.Api.Commands.V1;

[ApiVersion(1)]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status500InternalServerError)]
[Route("api/v{v:apiVersion}/command")]
public class AuctionCommands : ControllerBase
{
    private readonly IBaseRepository<Core.Models.Auction> _auctionRepository;
    private readonly IBidRepository _bidRepository;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;
    private readonly IVehicleService _vehicleService;

    public AuctionCommands(
        IBaseRepository<Core.Models.Auction> auctionRepository,
        IBidRepository bidRepository,
        IMapper mapper,
        IServiceProvider serviceProvider, 
        IVehicleService vehicleService)
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
        _mapper = mapper;
        _serviceProvider = serviceProvider;
        _vehicleService = vehicleService;
    }

    [HttpPost("create/auction")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<AuctionResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAuctionRequest createAuctionRequest)
    {
        var requestValidator = _serviceProvider.GetRequiredService<IValidator<CreateAuctionRequest>>();
        await requestValidator.ValidateAndThrowAsync(createAuctionRequest);

        var vehicle = await _vehicleService.GetById(createAuctionRequest.VehicleId);
        
        if ((vehicle as Vehicle)!.InAuction)
        {
            throw new VehicleAlreadyInAuctionException(createAuctionRequest.VehicleId);
        }

        var auction = new Core.Models.Auction(createAuctionRequest.VehicleId, Guid.NewGuid());
        var bid = new Bid(createAuctionRequest.StartBidValue, auction.Id, Guid.NewGuid());
        auction.SetStartingBid(bid);
        
        await _vehicleService.LockForAuction(vehicle);
        await _auctionRepository.Add(auction);
        await _bidRepository.Add(bid);

        return CreatedAtRoute(nameof(AuctionQueries.GetAuctionById), new { auctionId = auction.Id }, _mapper.Map<AuctionResponse>(auction));
    }

    [HttpPatch("create/auction/{auctionId:guid}/bid")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBid([FromRoute] Guid auctionId, [FromBody] CreateAuctionBidRequest createAuctionBidRequest)
    {
        var requestValidator = _serviceProvider.GetRequiredService<IValidator<CreateAuctionBidRequest>>();
        await requestValidator.ValidateAndThrowAsync(createAuctionBidRequest);
        
        var auction = await _auctionRepository.GetById(auctionId);
        
        if (auction is null)
        {
            throw new EntityNotFoundException(nameof(Core.Models.Auction), auctionId);
        }

        if (!auction.Active)
        {
            throw new AuctionClosedException(auctionId);
        }

        var bid = new Bid(createAuctionBidRequest.BidValue, auction.Id, Guid.NewGuid());
        await _bidRepository.Add(bid);

        auction.AddNewBid(bid);
        await _auctionRepository.Update(auction);

        return AcceptedAtRoute(nameof(AuctionQueries.GetAuctionById), new { id = auction.Id });
    }

    [HttpPatch("finish/auction/{auctionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FinishAuction([FromRoute] Guid auctionId)
    {
        var auction = await _auctionRepository.GetById(auctionId);
        if (auction is null)
        {
            throw new EntityNotFoundException(nameof(Core.Models.Auction), auctionId);
        }

        if (auction.Active)
        {
            var vehicle = await _vehicleService.GetById(auction.VehicleId);
            auction.Finish();
            await _auctionRepository.Update(auction);
            _vehicleService.ReleaseForAuction(vehicle);
        }

        return NoContent();
    }
}