using System.Net.Mime;
using Asp.Versioning;
using AutoMapper;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Core.Responses;
using Car.Auction.Management.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Car.Auction.Management.Api.Queries.V1;

[ApiVersion(1)]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/v{v:apiVersion}/manufacturers")]
public class VehicleManufacturerQueries : ControllerBase
{
    private readonly IBaseRepository<VehicleManufacturer> _manufacturerRepository;
    private readonly IMapper _mapper;
    
    public VehicleManufacturerQueries(
        IBaseRepository<VehicleManufacturer> manufacturerRepository, 
        IMapper mapper)
    {
        _manufacturerRepository = manufacturerRepository;
        _mapper = mapper;
    }
        
    [HttpGet(Name = "GetAllManufacturers")]
    [ProducesResponseType<List<VehicleManufacturerResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllManufacturers()
    {
        var manufacturers = await _manufacturerRepository.GetAll();
        return Ok(_mapper.Map<List<VehicleManufacturerResponse>>(manufacturers));
    }
}