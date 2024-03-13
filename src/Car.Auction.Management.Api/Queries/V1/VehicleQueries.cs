using System.Net.Mime;
using Asp.Versioning;
using AutoMapper;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Core.Responses;
using Car.Auction.Management.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Car.Auction.Management.Api.Queries.V1;

[ApiVersion(1)]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/v{v:apiVersion}/vehicles/")]
public class VehicleQueries : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IVehicleService _vehicleService;

    public VehicleQueries(
        IMapper mapper,
        IVehicleService vehicleService)
    {
        _mapper = mapper;
        _vehicleService = vehicleService;
    }
    
    [HttpGet]
    [ProducesResponseType<List<GenericVehicleResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllVehicles()
    {
        var vehicles = await _vehicleService.GetAll();
        return Ok(_mapper.Map<List<GenericVehicleResponse>>(vehicles));
    }
    
    [HttpGet("hatchbacks/{id:guid}", Name = "GetHatchbackById")]
    [ProducesResponseType<HatchbackResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHatchbackById([FromRoute]Guid id)
    {
        var hatchback = await _vehicleService.GetById<Hatchback>(id);
        return Ok(_mapper.Map<HatchbackResponse>(hatchback));
    }
    
    [HttpGet("hatchbacks")]
    [ProducesResponseType<List<HatchbackResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllHatchbacks()
    {
        var hatchbacks = await _vehicleService.GetAll<Hatchback>();
        return Ok(_mapper.Map<List<HatchbackResponse>>(hatchbacks));
    }
    
    [HttpGet("sedans/{id:guid}", Name = "GetSedanById")]
    [ProducesResponseType<SedanResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSedanById([FromRoute]Guid id)
    {
        var sedan = await _vehicleService.GetById<Sedan>(id);
        return Ok(_mapper.Map<SedanResponse>(sedan));
    }
    
    [HttpGet("sedans")]
    [ProducesResponseType<List<SedanResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSedans()
    {
        var sedans = await _vehicleService.GetAll<Sedan>();
        return Ok(_mapper.Map<List<SedanResponse>>(sedans));
    }
    
    [HttpGet("suvs/{id:guid}", Name = "GetSuvById")]
    [ProducesResponseType<SuvResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSuvById([FromRoute]Guid id)
    {
        var suv = await _vehicleService.GetById<Suv>(id);
        return Ok(_mapper.Map<SuvResponse>(suv));
    }
    
    [HttpGet("suvs")]
    [ProducesResponseType<List<SuvResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSuvs()
    {
        var suvs = await _vehicleService.GetAll<Suv>();
        return Ok(_mapper.Map<List<SuvResponse>>(suvs));
    }
    
    [HttpGet("trucks/{id:guid}", Name = "GetTruckById")]
    [ProducesResponseType<TruckResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTruckById([FromRoute]Guid id)
    {
        var truck = await _vehicleService.GetById<Truck>(id);
        return Ok(_mapper.Map<TruckResponse>(truck));
    }
    
    [HttpGet("trucks")]
    [ProducesResponseType<List<TruckResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTrucks()
    {
        var trucks = await _vehicleService.GetAll<Truck>();
        return Ok(_mapper.Map<List<TruckResponse>>(trucks));
    }
}