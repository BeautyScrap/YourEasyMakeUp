namespace YourEasyRent.Services
{
    public class BotState
    {
        public long ChatId { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }

        //public MenuStatus Status { get; set; }

        public bool PropertiesAreFilled()
        {
            return !string.IsNullOrEmpty(Brand) && !string.IsNullOrEmpty(Category) && ChatId != null;
        }

        //public enum MenuStatus
        //{
        //    Started,
        //    MainMenu,
        //    BrandMenu,
        //    BrandChosen,
        //    CategoryMenu,
        //    CategoryChosen
        //}

    }
}
