using AutoMapper;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Core.Responses;

namespace Car.Auction.Management.Api.Core;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Hatchback, HatchbackResponse>()
            .ForMember(m => m.ModelManufacturer, opt => opt
                .MapFrom(entity => entity.VehicleModel.Manufacturer.Name))
            .ForMember(m => m.ModelName, opt => opt
                .MapFrom(entity => entity.VehicleModel.Name));
        
        CreateMap<Sedan, SedanResponse>()
            .ForMember(m => m.ModelManufacturer, opt => opt
                .MapFrom(entity => entity.VehicleModel.Manufacturer.Name))
            .ForMember(m => m.ModelName, opt => opt
                .MapFrom(entity => entity.VehicleModel.Name));
        
        CreateMap<Suv, SuvResponse>()
            .ForMember(m => m.ModelManufacturer, opt => opt
                .MapFrom(entity => entity.VehicleModel.Manufacturer.Name))
            .ForMember(m => m.ModelName, opt => opt
                .MapFrom(entity => entity.VehicleModel.Name));
        
        CreateMap<Truck, TruckResponse>()
            .ForMember(m => m.ModelManufacturer, opt => opt
                .MapFrom(entity => entity.VehicleModel.Manufacturer.Name))
            .ForMember(m => m.ModelName, opt => opt
                .MapFrom(entity => entity.VehicleModel.Name));
        
        CreateMap<Vehicle, GenericVehicleResponse>()
            .ForMember(m => m.ModelManufacturer, opt => opt
                .MapFrom(entity => entity.VehicleModel.Manufacturer.Name))
            .ForMember(m => m.ModelName, opt => opt
                .MapFrom(entity => entity.VehicleModel.Name));
        
        CreateMap<VehicleManufacturer, VehicleManufacturerResponse>();
        
        CreateMap<Models.Auction, AuctionResponse>()
            .ConstructUsing(entity => 
                new AuctionResponse(
                    GetDateOnly(entity.CreationDate), 
                    GetDateOnly(entity.LastUpdate), 
                    GetTimeOnly(entity.LastUpdate), 
                    entity.VehicleId, 
                    GetHighestBidValue(entity.HighestBid), 
                    entity.Active,
                    entity.Id));
        
        CreateMap<Bid, BidResponse>()
            .ForMember(m => m.CreationDate, opt => opt
                .MapFrom(entity => DateOnly.FromDateTime(entity.CreationDate)))
            .ForMember(m => m.CreationTime, opt => opt
                .MapFrom(entity => TimeOnly.FromDateTime(entity.CreationDate)));
    }

    private static decimal GetHighestBidValue(Bid? bid)
    {
        return bid?.Value ?? 0;
    }

    private static string GetDateOnly(DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime).ToString("dd-MM-yyy");
    }

    private static string GetTimeOnly(DateTime dateTime)
    {
        return TimeOnly.FromDateTime(dateTime).ToString("HH:mm:ss");
    }
}