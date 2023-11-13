using HtmlAgilityPack;
using System;
using YourEasyRent.Entities;
using System.Text.Json;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;

namespace YourEasyRent.Services
{
    public class SephoraClient:IProductsSiteClient

    {
        private readonly string _baseUrl = $"https://www.sephora.de/";//  baseUrl стандартный адрес сайта
        private readonly HttpClient _httpClient;  // HttpClient - это класс, который предоставляет удобные методы для выполнения HTTP-запросов к веб-серверам и получения ответов от них. Он представляет собой клиент для работы с HTTP-ресурсами. HttpClient использует HttpClientHandler в качестве обработчика для обработки и выполнения запросов.      

        private readonly Dictionary<Section, string> sectionMapping = new()
        {
            [Section.Makeup] = "make-up"
        };
        public Site Site => Site.Sephora;
        public SephoraClient(HttpClient httpClient)
        {
            _httpClient =  httpClient; //  было new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);

        }
        public async Task<IEnumerable<Product>> FetchFromSectionAndPage(Section section, int pageNumber)

        {
            var url = GetSectionUrl(sectionMapping[section]);

            var htmlDocument = await GetHtmlPage(url);

            var productCardNodes = GetProductNodes(htmlDocument);

            var products =  await MapNodesToProduct(productCardNodes);

            return  products;


        }
        private string GetSectionUrl(string section)
        {
            return $"{_baseUrl}{section}/make-up-c302/";

        }

        private async Task<HtmlDocument> GetHtmlPage(string url) //создаем экземпляр HtmlDocument 
        {
            var sephoraProductResponce = await _httpClient.GetAsync(url);   // Отправляем GET-запрос на страницу с товарами
            var sephoraProductString = await sephoraProductResponce.Content.ReadAsStringAsync(); // читаем его как строку
            var htmlDocument = new HtmlDocument();// Создаем объект HtmlDocument и загружаем в него HTML-код страницы товара
            htmlDocument.LoadHtml(sephoraProductString); //  загружаем в него HTML-страницу с помощью метода LoadHtml()
            return htmlDocument;
        }

        private static List<HtmlNode> GetProductNodes(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//li[@class='grid-tile']").ToList(); // было "//li[@class = 'grid-tile']", проверить еще раз , может проблема в слишком долгом пути  / было ("//li[@class='grid-tile']/div[@class='product-tile product-tile-with-legal clickable omnibus-tile']")
        }

        private  async Task<IEnumerable<Product>> MapNodesToProduct(List<HtmlNode> productCardNodes)
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

        private async Task<Product> HtmlToProduct(HtmlNode node) // педставляет определение метода с именем HtmlToProduct и  возвращает объект типа Product.
        {
           
            var idNode = node.SelectSingleNode(".//div[@class='product-tile product-tile-with-legal clickable omnibus-tile']/@data-itemid")?.GetAttributeValue("data-itemid", "");
            var brandNode = node.SelectSingleNode(".//span[@class='product-brand']")?.InnerText;
            var nameNode = node.SelectSingleNode(".//h3[@class='product-title bidirectional']/span[@class='summarize-description title-line title-line-bold']")?.InnerText;
            var priceString = node.SelectSingleNode(".//span[@class='price-sales-standard' or @class='product-min-price']")?.InnerText.Trim().Replace(" &#8364;", "").Replace(" Ab:", ""); // Replace(",", "."); Replace(" €", "").Replace(",", ".")
            var imageUrlNode = node.SelectSingleNode(".//img[@class='product-first-img']/@src")?.GetAttributeValue("src", "");
            
            var url = node.SelectSingleNode(".//a[@class='product-tile-link']/@href")?.GetAttributeValue("href", "");

            var innerProductResponse = await _httpClient.GetAsync(url);
            var innerProductResponseString = await innerProductResponse.Content.ReadAsStringAsync();
            var productHtmlDocument = new HtmlDocument();
            productHtmlDocument.LoadHtml(innerProductResponseString);

            var categoryNode =  productHtmlDocument.DocumentNode.SelectSingleNode(".//div[@class='breadcrumb pdp-breadcrumb']//div[@class='breadcrumb-element'][4]/a/@title")?.GetAttributeValue("title", "");
            //node.SelectSingleNode("(//div[@class='breadcrumb pdp-breadcrumb']//div[@class='breadcrumb-element'][4]/a/@title)").GetAttributeValue("title", "");

            if (idNode == null || brandNode == null || nameNode == null || priceString == null || imageUrlNode == null || url == null || categoryNode == null)
            {
                return null; // Пропускаем node и возвращаем null
            }


            var product = new Product
            {
                SiteId = idNode,
                Brand = brandNode,
                Name = nameNode,
                Category = categoryNode,
                Price = decimal.Parse(priceString),
                Url = url,
                ImageUrl = imageUrlNode

            };
            return product;




        }
    }
}
