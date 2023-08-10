﻿using YourEasyRent.Entities;
using System.Text.Json;
using YourEasyRent.Entities.Douglas;

namespace YourEasyRent.Services
{
    public class DouglasClient
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<Section, string> sectionMapping = new()
        {       
            [Section.makeup] = "make-up"
        };
        private readonly JsonSerializerOptions _options;
        //private static string _baseProductPageUrl = "https://www.douglas.de/de/p/";  

        public DouglasClient()
        {
            _httpClient = new HttpClient();
            _options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }


        public async Task<IEnumerable<Product>> FetchFromSection(Section section, int pageNumber)
        {
            //var url = GetSectionUrl(sectionMapping[section], pageNumber);

            var url = $"https://www.douglas.de/jsapi/v2/products/search/category/03?currentPage={pageNumber}&pageSize=47&fields=FULL";
            var douglasHTTPResponce = await _httpClient.GetAsync(url);   

            var douglasRawJsonString = await douglasHTTPResponce.Content.ReadAsStringAsync();

            var douglasProducts = JsonSerializer.Deserialize<DouglasResponse>(douglasRawJsonString, _options);

            var products = douglasProducts.Products.Select(p => ToProduct(p));

            return products;
        }

        private static Product ToProduct(DouglasProduct p)  // переложить из douglas.product  в new product , создать в дуглас файл с продуктами, которые будут называться как "code, baseProductName,imageURL 
        {
            var product = new Product  // а тут уже поставить сопоставление со своими названиями с названиями, которые выдает дуглас( как в золотом яблоке)
            {
                SiteId = p.Code,
                Url =  p.Url,
                Price = p.Price.Value,               //_baseProductPageUrl*/
                ImageUrl = p.Images.First().Url,
                Brand = p.Brand.Name,
                Name = p.BaseProductName

            };
            return product; 

                // переложить из douglas.product  в new product , создать в дуглас файл с продуктами, которые будут называться как "code, baseProductName,imageURL 
            
        }// а тут уже поставить сопоставление со своими названиями с названиями, которые выдает дуглас( как в золотом яблоке)

        internal Task FetchFromSection(Section section)
        {
            throw new NotImplementedException();
        }
    }

}
