using System.Text.Json;
namespace SubscriberAPI.Domain
{
    public class SudscriberDto
    {
        public string? UserId { get; set; }
        public string? ChatId { get; set; }
        //public string? Category { get; set; } 
        public string? Brand { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Url { get; set; }
    }
}
