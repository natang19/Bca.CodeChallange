namespace Car.Auction.Management.Api.Core.CustomExceptions;

public class EntityNotFoundException(string entityType, Guid entityId) : Exception($"{entityType} {entityId} not found");