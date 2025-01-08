namespace YourEasyRent.Contracts.ProductForSubscription
{
    public record ProductForSubscriptionRequest(
        string UserId,
        string Brand,
        string Name,
        decimal Price,
        string? Url,
        string? UrlImage);
    
}
