namespace YourEasyRent.Services;

public interface ITelegramActionsHandler
{
    Task<string> GetFilteredProductsMessage(string brand, string category);
}
