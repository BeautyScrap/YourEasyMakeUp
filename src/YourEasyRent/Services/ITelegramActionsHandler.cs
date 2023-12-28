namespace YourEasyRent.Services
{
    public interface ITelegramActionsHandler
    {
       
        Task<IEnumerable<string>> GetFilteredProductsMessage(string brand, string category);
    }
}
