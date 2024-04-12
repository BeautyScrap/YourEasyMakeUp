namespace YourEasyRent.Services.Keyboard
{
    public interface ITelegramSender
    {
        Task SendMainMenu(long chatId);
        Task SendCategoryMenu(long chatId);// categoryList откуда мы должны брать эти листы categoryList итд?
        Task SendBrandMenu(long chatId);// brandList
        Task SendResults();  //string results
        Task SendMenuAfterResult(long chatId);
    }
}
