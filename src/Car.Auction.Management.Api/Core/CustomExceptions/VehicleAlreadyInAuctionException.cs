using Car.Auction.Management.Api.Core.Models;
using Car.Auction.Management.Api.Core.Requests;

namespace Car.Auction.Management.Api.Core.CustomExceptions;

public class VehicleAlreadyInAuctionException(Guid vehicleId) : EntityCustomValidationException(nameof(Vehicle), nameof(Vehicle.Id), vehicleId.ToString(), "Already in auction");