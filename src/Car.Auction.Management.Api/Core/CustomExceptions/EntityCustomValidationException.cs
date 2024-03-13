namespace Car.Auction.Management.Api.Core.CustomExceptions;

public abstract class EntityCustomValidationException : Exception
{
    protected EntityCustomValidationException(string modelName, string propertyName, string propertyValue, string errorMessage) :
        base($"{modelName}: {propertyName} {propertyValue} {errorMessage}")
    {
        
    }
    
    protected EntityCustomValidationException(string modelName, string propertyName, string errorMessage) : 
        base($"{modelName}: {propertyName} {errorMessage}")
    {
        
    }
    
    protected EntityCustomValidationException(string modelName, Guid modelId, string errorMessage) : 
        base($"{modelName} {modelId}: {errorMessage}")
    {
        
    }
}