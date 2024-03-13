using System.Net.Mime;
using Asp.Versioning;
using AutoMapper;
using Car.Auction.Management.Api.Core.CustomExceptions;
using Car.Auction.Management.Api.Core.Responses;
using Car.Auction.Management.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Car.Auction.Management.Api.Queries.V1;

[ApiVersion(1)]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/v{v:apiVersion}/auctions")]
public class AuctionQueries : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IBaseRepository<Core.Models.Auction> _auctionRepository;
    private readonly IBidRepository _bidRepository;

    public AuctionQueries(
        IMapper mapper, 
        IBaseRepository<Core.Models.Auction> auctionRepository, 
        IBidRepository bidRepository)
    {
        _mapper = mapper;
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
    }

    [HttpGet(Name = "GetAllAuctions")]
    [ProducesResponseType<List<AuctionResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAuctions()
    {
        var auctions = await _auctionRepository.GetAll();
        return Ok(_mapper.Map<List<AuctionResponse>>(auctions));
    }
    
    [HttpGet("{auctionId:guid}", Name = "GetAuctionById")]
    [ProducesResponseType<AuctionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuctionById(Guid auctionId)
    {
        var auction = await _auctionRepository.GetById(auctionId);
        
        if (auction is null)
        {
            throw new EntityNotFoundException(nameof(Core.Models.Auction), auctionId);
        }
        
        return Ok(_mapper.Map<List<AuctionResponse>>(auction));
    }
    
    [HttpGet("{auctionId:guid}/bids")]
    [ProducesResponseType<List<BidResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBidsOfAuction(Guid auctionId)
    {
        var bids = await _bidRepository.GetBidsByAuctionId(auctionId);
        return Ok(_mapper.Map<List<BidResponse>>(bids));
    }
}