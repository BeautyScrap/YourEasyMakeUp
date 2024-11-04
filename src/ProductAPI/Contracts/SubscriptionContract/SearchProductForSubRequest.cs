namespace ProductAPI.Contracts.ProductForSubscription
{
    public record SearchProductForSubRequest(
        string UserId,
        string Name,
        decimal Price);
 
}
