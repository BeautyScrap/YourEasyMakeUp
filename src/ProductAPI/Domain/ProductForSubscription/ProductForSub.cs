namespace ProductAPI.Domain.ProductForSubscription

{
    public class ProductForSub
    {
        public string UserId { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }//AK TODO добавть дату извеннения наверно
        public ProductForSub() { }

        public static ProductForSub CreateProductForSearch(string userId, string name, decimal price)
        {
            ProductForSub product = new ProductForSub()
            {
                UserId = userId,
                Name = name,
                Price = price
            };
            return product;
        }

        public ProductForSubDto ToDto()
        {
            ProductForSubDto subscribersDto = new ProductForSubDto()
            {
                UserId = UserId,
                Name = Name,
                Price = Price
            };
            return subscribersDto;
        }
    }
}

