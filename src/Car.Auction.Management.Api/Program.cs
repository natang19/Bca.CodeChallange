using System.Text.Json;
using Asp.Versioning;
using Car.Auction.Management.Api.Core;
using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Core.Requests;
using Car.Auction.Management.Api.Middlewares;
using Car.Auction.Management.Api.Repositories;
using Car.Auction.Management.Api.Services;
using FluentValidation;

namespace Car.Auction.Management.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        RegisterRepositories(builder.Services);
        RegisterServices(builder.Services);
        ConfigureValidators(builder.Services);

        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        builder.Services.AddEndpointsApiExplorer();
        ConfigureSwagger(builder.Services);
        
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseExceptionHandler();

        app.Run();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddSingleton<IVehicleRepository<Hatchback>, HatchbackRepository>();
        services.AddSingleton<IVehicleRepository<Sedan>, SedanRepository>();
        services.AddSingleton<IVehicleRepository<Suv>, SuvRepository>();
        services.AddSingleton<IVehicleRepository<Truck>, TruckRepository>();
        services.AddSingleton<IBaseRepository<VehicleManufacturer>, VehicleManufacturerRepository>();
        services.AddSingleton<IBaseRepository<Core.Models.Auction>, AuctionRepository>();
        services.AddSingleton<IBidRepository, BidRepository>();
    }

    private static void ConfigureValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateVehicleRequest>, CreateVehicleRequestValidator>();
        services.AddScoped<IValidator<CreateVehicleManufacturerRequest>, RegisterVehicleManufacturerRequestValidator>();
        services.AddScoped<IValidator<CreateTruckRequest>, CreateTruckRequestValidator>();
        services.AddScoped<IValidator<CreateSuvRequest>, CreateSuvRequestValidator>();
        services.AddScoped<IValidator<CreateSedanRequest>, CreateSedanRequestValidator>();
        services.AddScoped<IValidator<CreateHatchbackRequest>, CreateHatchbackRequestValidator>();
        services.AddScoped<IValidator<CreateAuctionRequest>, CreateAuctionRequestValidator>();
        services.AddScoped<IValidator<CreateAuctionBidRequest>, CreateAuctionBidRequestValidator>();
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}