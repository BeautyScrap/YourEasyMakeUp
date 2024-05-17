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

        public async Task<bool> IsReadyToSearchMethod(string userId) // метод проверяет, что нужные поля (бренд и категория) у пользователя уже заполнены
        {
            var resultOfFields = await _userStateRepository.CheckFieldsFilledForUser(userId);
            return resultOfFields;
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
        public async Task<IEnumerable<string>> GetFilteredProductsMessage(string brand, string category) //  не знаю как для этого метода "распаковать" данные(бренд и категорию) из Листа,
                                                                                                         //  чтобы потом засунить их в аргемент метода GetProductsByBrandAndCategory
        {
            var products = await _productRepository.GetProductsByBrandAndCategory(brand, category);
            {
                var productStrings = products.Select(p =>
            $"*{p.Brand}*\n" +
            $"{p.Name}\n" +
            $"{p.Category}\n" +
            $"{p.Price}\n" +
            $"[.]({p.ImageUrl})\n" +
            $"[Ссылка на продукт]({p.Url})");
                return productStrings;
            }

        }
    }
}
