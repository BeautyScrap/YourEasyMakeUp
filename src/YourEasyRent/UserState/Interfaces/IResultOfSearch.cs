namespace YourEasyRent.UserState.Interfaces
{
    public interface IResultOfSearch
    {
        Task<bool> IsReadyToSearchMethod(string userId);
        Task<IEnumerable<string>> TakeFilterdProductFromDb(string userId);
        Task<IEnumerable<string>> GetFilteredProductsMessage(string brand, string category);
    }
}
