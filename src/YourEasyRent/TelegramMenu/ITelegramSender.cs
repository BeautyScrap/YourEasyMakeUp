namespace YourEasyRent.TelegramMenu
{
    public interface ITelegramSender
    {
        Task SendMainMenu(long chatId);
        Task SendCategoryMenu(long chatId);// categoryList откуда мы должны брать эти листы categoryList итд?
        Task SendBrandMenu(long chatId);// brandList
        Task SendResults();  //string results. это аналог на Task<IEnumerable<string>> SendAllResult, но поскольку это не клавиатура, а просто результат стрики 
        // из бд, этот метод может быть аналогм public void IsFiniShed()// в USS, тк аргументы бренд и категорию можно брать от туда??
        Task SendMenuAfterResult(long chatId);
    }
}
