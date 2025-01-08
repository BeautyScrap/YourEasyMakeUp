namespace TelegramBotAPI.Contracts
{
    public class FoundProductResultResponse
    {
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
    }
}
