namespace ProductAPI.Domain.Product
{
    public class ProductDto
    {
        public Site? Site{ get; set; }
        public string? Brand { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }   
        public string? Url { get; set; }
        public string? ImageUrl { get; set; }
    }
}
