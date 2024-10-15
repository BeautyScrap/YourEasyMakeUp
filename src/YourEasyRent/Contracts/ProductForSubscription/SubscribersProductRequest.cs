namespace YourEasyRent.Contracts.ProductForSubscription
{
    public record SubscribersProductRequest(
        string UserId,
        string ChatId,
        string Brand,
        string Name,
        decimal Price);
    
}
