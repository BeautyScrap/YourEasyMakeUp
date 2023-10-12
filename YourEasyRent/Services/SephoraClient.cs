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

namespace YourEasyRent.Services
{
    public class SephoraClient:ISephoraProductSiteClient

    {
        private readonly string _baseUrl = $"https://www.sephora.de/";//  baseUrl стандартный адрес сайта
        private string _innerProductUrl = "";
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
        public async Task<IEnumerable<Product>> FetchFromSephoraSection(Section section, int pageNumber)

        {
            var url = GetSectionUrl(sectionMapping[section]);

            var htmlDocument = await GetHtmlPage(url);

            var productCardNodes = GetProductNode(htmlDocument);

            var products = MapNodesToProduct(productCardNodes);

            return products;


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
        private static List<HtmlNode> GetProductNode(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//li[@class='grid-tile']").ToList(); // было "//li[@class = 'grid-tile']", проверить еще раз , может проблема в слишком долгом пути  / было ("//li[@class='grid-tile']/div[@class='product-tile product-tile-with-legal clickable omnibus-tile']")
        }

        private IEnumerable<Product> MapNodesToProduct(List<HtmlNode> productCardNodes)
        {
            return productCardNodes.Select(HtmlToProduct);
        }




        private  Product HtmlToProduct(HtmlNode node) // педставляет определение метода с именем HtmlToProduct и  возвращает объект типа Product.
        {
            var idNode = node.SelectSingleNode("//li[@class='grid-tile']/div[@class='product-tile product-tile-with-legal clickable omnibus-tile']/@data-itemid").GetAttributeValue("data-itemid", "");
            var brandNode = node.SelectSingleNode("//li[@class='grid-tile']//span[@class='product-brand']").InnerText;
            var nameNode = node.SelectSingleNode("//li[@class='grid-tile']//h3[@class='product-title bidirectional']/span[@class='summarize-description title-line title-line-bold']").InnerText;
            //var breadcrumbLabelForCategoryNode = node.SelectSingleNode("//li[@class='grid-tile']/div[@class='product-tile product-tile-with-legal clickable omnibus-tile']/@data-tcproduct").GetAttributeValue("data-tcproduct", "");                     
            //var categoryNode = Regex.Match(breadcrumbLabelForCategoryNode, @"product_breadcrumb_label&quot;:&quot;([^&]+)&quot;").Groups[1].Value.Split('/')[2];

            var priceString = node.SelectSingleNode("//li[@class='grid-tile']//span[@class='price-sales-standard']").InnerText.Trim().Replace(" &#8364;", ""); // Replace(",", "."); Replace(" €", "").Replace(",", ".")

            var imageUrlNode = node.SelectSingleNode("//li[@class='grid-tile']//img[@class='product-first-img']/@src").GetAttributeValue("src", "");
            
            var url = node.SelectSingleNode("//li[@class='grid-tile']//a[@class='product-tile-link']/@href").GetAttributeValue("href", "");

            var innerProductResponse = _httpClient.GetAsync(url);
            var innerProductResponseString = innerProductResponse.ToString();  
            var productHtmlDocument = new HtmlDocument();
            productHtmlDocument.LoadHtml(innerProductResponseString);

            var categoryNode = node.SelectSingleNode("(//div[@class='breadcrumb-element'][4]/a[@title])[1]").InnerText;



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
