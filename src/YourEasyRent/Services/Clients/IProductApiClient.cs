namespace TelegramBotAPI.Services
{
    public interface IProductApiClient
    {
        Task<List<string>> GetBrandForMenu(string chatId ,int limit);

    }
}
