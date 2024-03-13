namespace Car.Auction.Management.Api.Core.CustomExceptions;

public class DuplicatedIdException(string modelName) : Exception($"{modelName}: Id already created");