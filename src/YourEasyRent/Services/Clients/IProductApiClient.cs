namespace TelegramBotAPI.Services
{
    public interface IProductApiClient
    {
        Task<List<string>> GetBrandForMenu(string chatId ,int limit);
        //Task<IEnumerable<string>> GetProductsResultForUser(List<string> listWithResult);
        Task<string> GetOneProductResultForUser(List<string> listWithResult);
        Task<(string? Brand, string? Name, decimal Price, string? Url)> GetProductResultTuple(List<string> listWithResult);
    }
}
