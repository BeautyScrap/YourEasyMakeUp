﻿



namespace YourEasyRent.Services
{
    public interface ITelegramActionsHandler
    {
        Task<string> GetFilteredProductsMessage(string brand, decimal price);
    }
}
