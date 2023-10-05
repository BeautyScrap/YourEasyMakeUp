namespace YourEasyRent.Services;

public interface ITelegramActionsHandler
{
    Task ShowFilteredProducts(long chatId, string category, string brand);
}
