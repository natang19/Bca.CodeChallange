using Car.Auction.Management.Api.Core.Requests;

namespace Car.Auction.Management.Api.Core.CustomExceptions;

public class ManufacturerNotRegisteredException(Guid manufacturerId)
    : EntityCustomValidationException(nameof(CreateVehicleRequest), nameof(CreateVehicleRequest.ManufacturerId), manufacturerId.ToString(), "not registered");

