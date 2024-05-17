using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.UserState.Interfaces;

namespace YourEasyRent.UserState
{
    public class ResultOfSearch : IResultOfSearch
    {
        private readonly IUserStateRepository _userStateRepository;
        private readonly IProductRepository _productRepository;
        public bool IsReadyToSearch { get; private set; }

        public ResultOfSearch(IUserStateRepository userStateRepository, IProductRepository productRepository)
        {
            _userStateRepository = userStateRepository;
            _productRepository = productRepository;

        }

        public async Task<IEnumerable<string>> TakeFilterdProductFromDb(string userId) // забирает заполненные поля из базы User
        {
            var fieldsForSearch = await _userStateRepository.GetFilteredProducts(userId);
            var listWithfieldsToResult = new List<string>
            {
                fieldsForSearch.ToString(),
            };
            return listWithfieldsToResult;

        }

    }
}
