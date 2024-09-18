namespace SubscriberAPI.Contracts
{
    public class SubscriptionResponse
    {
        public string UserId { get; set; }
        public string? ChatId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Url { get; set; }
    }
}
