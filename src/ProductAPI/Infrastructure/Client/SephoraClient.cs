using HtmlAgilityPack;
using ProductAPI.Domain.Product;
using System.Net.Http.Headers;

namespace ProductAPI.Infrastructure.Client
{
    public class SephoraClient : IProductsSiteClient
    {
        private readonly string _baseUrl = $"https://www.sephora.de/shop";
        private readonly HttpClient _httpClient;
        private readonly Dictionary<Section, string> sectionMapping = new()
        {
            [Section.Makeup] = "make-up"//  думаю можно будет прописывать значение прям с цифрами make-up-c302 для разных разделов
        };
        public Site Site => Site.Sephora;
        public SephoraClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true
            };
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
        }


        public async Task<IEnumerable<Product>> FetchFromSectionAndPage(Section section, int pagenumber)
        {
            var url = GetSectionUrl(sectionMapping[section], pagenumber);
            var htmlDocument = await GetHtmlPage(url);
            var productCardNodes = GetProductNodes(htmlDocument);
            var products = await MapNodesToProduct(productCardNodes);
            return products;
        }
        private string GetSectionUrl(string section, int pageNumber)
        {
            return $"{_baseUrl}/{section}-c302/?page={pageNumber}";
        }

        private async Task<HtmlDocument> GetHtmlPage(string url)
        {
            var sephoraProductResponce = await _httpClient.GetAsync(url);
            var sephoraProductString = await sephoraProductResponce.Content.ReadAsStringAsync();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(sephoraProductString);
            return htmlDocument;
        }

        private static List<HtmlNode> GetProductNodes(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//li[@class='grid-tile' or @class='tc-push-products hide']").ToList();
        }

        private async Task<IEnumerable<Product>> MapNodesToProduct(List<HtmlNode> productCardNodes)
        {
            var products = new List<Product>();
            foreach (var node in productCardNodes)
            {
                var product = await HtmlToProduct(node);
                if (product == null)
                {
                    continue;
                }
                products.Add(product);
            }
            return products;
        }

        private async Task<Product> HtmlToProduct(HtmlNode node)
        {

            //var idNode = node.SelectSingleNode(".//div[@class='product-tile product-tile-with-legal clickable omnibus-tile']/@data-itemid")?.GetAttributeValue("data-itemid", "");
            var brandNode = node.SelectSingleNode(".//span[@class='product-brand']")?.InnerText;
            var nameNode = node.SelectSingleNode(".//h3[@class='product-title bidirectional']/span[@class='summarize-description title-line title-line-bold']")?.InnerText;
            var priceString = node.SelectSingleNode(".//span[@class='price-sales-standard' or @class='product-sales-price black-price']")?.InnerText.Trim().Replace(" &#8364;", "").Replace(" Ab:", "");
            var imageUrlNode = node.SelectSingleNode(".//img[@class='product-first-img']/@src")?.GetAttributeValue("src", "");
            var urlNode = node.SelectSingleNode(".//a[@class='product-tile-link']/@href")?.GetAttributeValue("href", "");

            var innerProductResponse = await _httpClient.GetAsync(urlNode);
            var innerProductResponseString = await innerProductResponse.Content.ReadAsStringAsync();
            var productHtmlDocument = new HtmlDocument();
            productHtmlDocument.LoadHtml(innerProductResponseString);

            var categoryNode = productHtmlDocument.DocumentNode.SelectSingleNode(".//div[@class='breadcrumb pdp-breadcrumb']//div[@class='breadcrumb-element'][4]/a/@title")?.GetAttributeValue("title", "");

            if ( brandNode == null || nameNode == null || priceString == null || imageUrlNode == null || urlNode == null || categoryNode == null)
            {
                return null;
            }
            var price = decimal.Parse(priceString);

            var product = Product.CreateProduct(Site.Sephora ,brandNode, nameNode, price, categoryNode, urlNode, imageUrlNode);
            return product;
        }
    }
}
