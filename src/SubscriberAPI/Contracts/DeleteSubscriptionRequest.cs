namespace SubscriberAPI.Contracts
{
    public class DeleteSubscriptionRequest
    {
        public string UserId { get; set; }
        public string? ChatId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
    }
}
