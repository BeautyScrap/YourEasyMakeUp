namespace YourEasyRent.Entities.ProductForSubscription
{
    public record ProductForSubscriptionRequest(
        string UserId,
        string ChatId,
        string Brand,
        string Name,
        decimal Price,
        string Url
        );
}
