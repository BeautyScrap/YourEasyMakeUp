namespace SubscriberAPI.Contracts
{
    public record SubscriptionRequest(
        string UserId,
        string ChatId,
        string Brand,
        string Name,
        decimal Price,
        string Url
        );   
}

