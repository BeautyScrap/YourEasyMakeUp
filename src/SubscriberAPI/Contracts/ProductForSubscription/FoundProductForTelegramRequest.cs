namespace SubscriberAPI.Contracts.ProductForSubscription
{
    public class FoundProductForTelegramRequest
    {
        public string UserId { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public string UrlImage { get; set; }
    }
}
