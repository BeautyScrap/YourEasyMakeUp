namespace YourEasyRent.Contracts.ProductForSubscription
{
    public record SubscribersProductRequest(
        string UserId,
        string Brand,
        string Name,
        decimal Price);
    
}
