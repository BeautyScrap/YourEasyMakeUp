using System.Text.Json;
namespace SubscriberAPI.Domain
{
    public class SubscriptionDto 
    {
        public string? UserId { get; set; }
        public string? ChatId { get; set; }
        public string? Brand { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Url { get; set; }
        public string? UrlImage { get; set; }
    }
}
