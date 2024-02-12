namespace YourEasyRent.Services
{
    public class BotState
    {
        public long ChatId { get; set; }
        public string Category { get; set; }    
        public string Brand { get; set; }   
        
        public MenuStatus Status { get; set; }

    }

    public enum MenuStatus
    {
        Started,
        MainMenu,
        BrandMenu,
        BrandChosen,
        CategoryMenu,
        CategoryChosen
    }
}
