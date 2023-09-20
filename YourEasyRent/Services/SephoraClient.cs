using HtmlAgilityPack;
using System;
using YourEasyRent.Entities;
using System.Text.Json;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Globalization;

namespace YourEasyRent.Services
{
    public class SephoraClient:ISephoraProductSiteClient

    {
        private readonly string _baseUrl = $"https://www.sephora.de/";//  baseUrl стандартный адрес сайта
        private readonly HttpClient _httpClient;  // HttpClient - это класс, который предоставляет удобные методы для выполнения HTTP-запросов к веб-серверам и получения ответов от них. Он представляет собой клиент для работы с HTTP-ресурсами. HttpClient использует HttpClientHandler в качестве обработчика для обработки и выполнения запросов.      

        private readonly Dictionary<Section, string> sectionMapping = new()
        {
            [Section.Makeup] = "shop"
        };
        public Site Site => Site.Sephora;
        public SephoraClient()
        {
            _httpClient =  new HttpClient();
            //_httpClient.BaseAddress = new Uri(_baseUrl);

        }
        public async Task<IEnumerable<Product>> FetchFromSephoraSection(Section section, int pageNumber)

        {
            var url = GetSectionUrl(sectionMapping[section]);

            var htmlDocument = await GetHtmlPage(url);

            var productCardNodes = GetProductNode(htmlDocument);

            var products = MapNodesToProduct(productCardNodes);

            return products;


        }

        private static IEnumerable<Product> MapNodesToProduct(List<HtmlNode> productCardNodes)
        {
            return productCardNodes.Select(HtmlToProduct);
        }

        private async Task<HtmlDocument> GetHtmlPage(string url) //создаем экземпляр HtmlDocument 
        {
            var sephoraProductResponce = await _httpClient.GetAsync(url);  //запрашиваем url страницы с квартирами 
            var sephoraProductString = await sephoraProductResponce.Content.ReadAsStringAsync(); // читаем его как строку
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(sephoraProductString); //  загружаем в него HTML-страницу с помощью метода LoadHtml()
            return htmlDocument;

        }

        private static List<HtmlNode> GetProductNode(HtmlDocument htmlDocument)
        {

            return htmlDocument.DocumentNode.SelectNodes("//li[@class='grid-tile']").ToList(); // было "//li[@class = 'grid-tile']", проверить еще раз , может проблема в слишком долгом пути  / было ("//li[@class='grid-tile']/div[@class='product-tile product-tile-with-legal clickable omnibus-tile']")
        }

        private string GetSectionUrl(string section)
        {
            return $"{_baseUrl}{section}/make-up-c302/";

        }

        private static Product HtmlToProduct(HtmlNode node) // педставляет определение метода с именем HtmlToProduct и  возвращает объект типа Product.
        {
            var idNode = node.SelectSingleNode("//li[@class='grid-tile']/div[@class='product-tile product-tile-with-legal clickable omnibus-tile']/@data-itemid").GetAttributeValue("data-itemid", "");
            var brandNode = node.SelectSingleNode("//li[@class='grid-tile']//span[@class='product-brand']").InnerText;
            var nameNode = node.SelectSingleNode("//li[@class='grid-tile']//h3[@class='product-title bidirectional']/span[@class='summarize-description title-line title-line-bold']").InnerText;


            var url = node.SelectSingleNode("//li[@class='grid-tile']//a[@class='product-tile-link']/@href").GetAttributeValue("href", "");

            var priceString = node.SelectSingleNode("//li[@class='grid-tile']//span[@class='price-sales-standard']").InnerText.Trim().Replace(" &#8364;", ""); // Replace(",", "."); Replace(" €", "").Replace(",", ".")



            var imageUrlNode = node.SelectSingleNode("//li[@class='grid-tile']//img[@class='product-first-img']/@src").GetAttributeValue("src", "");


            var product = new Product
            {
                SiteId = idNode,
                Brand = brandNode,
                Name = nameNode,
                Price = decimal.Parse(priceString),
                Url = url,
                ImageUrl = imageUrlNode

            };
            return product;




        }
    }
}
