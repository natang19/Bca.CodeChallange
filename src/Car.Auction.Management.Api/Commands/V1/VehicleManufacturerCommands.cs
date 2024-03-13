using System.Net.Mime;
using Asp.Versioning;
using AutoMapper;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Core.Requests;
using Car.Auction.Management.Api.Core.Responses;
using Car.Auction.Management.Api.Queries.V1;
using Car.Auction.Management.Api.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Car.Auction.Management.Api.Commands.V1;

[ApiVersion(1)]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status500InternalServerError)]
[ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<VehicleManufacturerResponse>(StatusCodes.Status201Created)]
[Route("api/v{v:apiVersion}")]
public class VehicleManufacturerCommands : ControllerBase
{
    private readonly IBaseRepository<VehicleManufacturer> _manufacturerRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateVehicleManufacturerRequest> _validator;

    public VehicleManufacturerCommands(
        IBaseRepository<VehicleManufacturer> manufacturerRepository, 
        IMapper mapper, 
        IValidator<CreateVehicleManufacturerRequest> validator)
    {
        _manufacturerRepository = manufacturerRepository;
        _mapper = mapper;
        _validator = validator;
    }
    
    [HttpPost("command/create/manufacturer")]
    public async Task<IActionResult> RegisterManufacturer([FromBody] CreateVehicleManufacturerRequest createVehicleManufacturerRequest)
    {
        await _validator.ValidateAndThrowAsync(createVehicleManufacturerRequest);
        
        var vehicleManufacturer = new VehicleManufacturer(createVehicleManufacturerRequest.Name, Guid.NewGuid());
        await _manufacturerRepository.Add(vehicleManufacturer);
        
        return CreatedAtRoute(nameof(VehicleManufacturerQueries.GetAllManufacturers), _mapper.Map<VehicleManufacturerResponse>(vehicleManufacturer));
    }
}