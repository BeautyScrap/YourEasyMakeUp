namespace YourEasyRent.TelegramMenu
{
    public interface ITelegramSender
    {
        Task SendMainMenu(string chatId);
        Task SendCategoryMenu(string chatId);// categoryList откуда мы должны брать эти листы categoryList итд?
        Task SendBrandMenu(string chatId);// brandList
        Task SendResults();  //string results. это аналог на Task<IEnumerable<string>> SendAllResult, но поскольку это не клавиатура, а просто результат стрики 
        // из бд, этот метод может быть аналогм public void IsFiniShed()// в USS, тк аргументы бренд и категорию можно брать от туда??
        Task SendMenuAfterResult(string chatId);
    }
}
