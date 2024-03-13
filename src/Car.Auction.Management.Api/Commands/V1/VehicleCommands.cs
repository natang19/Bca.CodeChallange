using System.Net.Mime;
using Asp.Versioning;
using AutoMapper;
using Car.Auction.Management.Api.Core.CustomExceptions;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Core.Requests;
using Car.Auction.Management.Api.Core.Responses;
using Car.Auction.Management.Api.Queries.V1;
using Car.Auction.Management.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Car.Auction.Management.Api.Commands.V1;

[ApiVersion(1)]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType<Dictionary<string, string>>(StatusCodes.Status400BadRequest)]
[Route("api/v{v:apiVersion}/command/create")]
public class VehicleCommands : ControllerBase
{
    private readonly IBaseRepository<VehicleManufacturer> _manufacturerRepository;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    public VehicleCommands(
        IMapper mapper, 
        IBaseRepository<VehicleManufacturer> manufacturerRepository, 
        IServiceProvider serviceProvider)
    {
        _mapper = mapper;
        _manufacturerRepository = manufacturerRepository;
        _serviceProvider = serviceProvider;
    }

    [HttpPost("hatchback")]
    [ProducesResponseType<HatchbackResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateHatchback([FromBody] CreateHatchbackRequest createHatchbackRequest)
    {
        var manufacturer = await GetVehicleManufacturer(createHatchbackRequest.ManufacturerId);
        
        var model = new VehicleModel(manufacturer, createHatchbackRequest.ModelName);
        var hatchback = new Hatchback(createHatchbackRequest.QuantityDoors, model,createHatchbackRequest.YearManufacture, Guid.NewGuid());
        
        var hatchbackRepository = _serviceProvider.GetRequiredService<IVehicleRepository<Hatchback>>();
        await hatchbackRepository.Add(hatchback);

        return CreatedAtRoute(nameof(VehicleQueries.GetHatchbackById),new { id = hatchback.Id }, _mapper.Map<HatchbackResponse>(hatchback));
    }
    
    [HttpPost("sedan")]
    [ProducesResponseType<SedanResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSedan([FromBody] CreateSedanRequest createSedanRequest)
    {
        var manufacturer = await GetVehicleManufacturer(createSedanRequest.ManufacturerId);
        
        var model = new VehicleModel(manufacturer, createSedanRequest.ModelName);
        var sedan = new Sedan(createSedanRequest.QuantityDoors, model,createSedanRequest.YearManufacture, Guid.NewGuid());
        
        var sedanRepository = _serviceProvider.GetRequiredService<IVehicleRepository<Sedan>>();
        await sedanRepository.Add(sedan);

        return CreatedAtRoute(nameof(VehicleQueries.GetSedanById),new { id = sedan.Id }, _mapper.Map<SedanResponse>(sedan));
    }
    
    [HttpPost("suv")]
    [ProducesResponseType<SuvResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSuv([FromBody] CreateSuvRequest createSuvRequest)
    {
        var manufacturer = await GetVehicleManufacturer(createSuvRequest.ManufacturerId);
        
        var model = new VehicleModel(manufacturer, createSuvRequest.ModelName);
        var suv = new Suv(createSuvRequest.QuantitySeats, createSuvRequest.QuantityDoors, model,createSuvRequest.YearManufacture, Guid.NewGuid());

        var suvRepository = _serviceProvider.GetRequiredService<IVehicleRepository<Suv>>();
        await suvRepository.Add(suv);

        return CreatedAtRoute(nameof(VehicleQueries.GetSuvById),new { id = suv.Id }, _mapper.Map<SuvResponse>(suv));
    }
    
    [HttpPost("truck")]
    [ProducesResponseType<TruckResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTruck([FromBody] CreateTruckRequest createTruckRequest)
    {
        var manufacturer = await GetVehicleManufacturer(createTruckRequest.ManufacturerId);
        
        var model = new VehicleModel(manufacturer, createTruckRequest.ModelName);
        var truck = new Truck(createTruckRequest.LoadCapacity, createTruckRequest.QuantityDoors, model,createTruckRequest.YearManufacture, Guid.NewGuid());

        var truckRepository = _serviceProvider.GetRequiredService<IVehicleRepository<Truck>>();
        await truckRepository.Add(truck);

        return CreatedAtRoute(nameof(VehicleQueries.GetTruckById),new { id = truck.Id }, _mapper.Map<TruckResponse>(truck));
    }

    private async Task<VehicleManufacturer> GetVehicleManufacturer(Guid manufacturerId)
    {
        var manufacturer = await _manufacturerRepository.GetById(manufacturerId);
        if (manufacturer is null)
        {
            throw new ManufacturerNotRegisteredException(manufacturerId);
        }

        return manufacturer;
    }
}