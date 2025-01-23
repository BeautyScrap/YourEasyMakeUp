namespace SubscriberAPI.Contracts.ProductForSubscription
{
    public class SearchSubProductRequest
    {
        public string UserId { get;  set; }
        public string Name { get;  set; }
        public decimal Price { get;  set; }
    }
}
