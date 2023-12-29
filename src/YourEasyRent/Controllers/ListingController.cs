using Microsoft.AspNetCore.Mvc;
using YourEasyRent.Entities;
using YourEasyRent.Services;
using YourEasyRent.DataBase.Interfaces;
using Section = YourEasyRent.Entities.Section;

namespace YourEasyRent.Controllers

{

    [ApiController]
    [Route("[controller]")] // было [Route("api/v1/[controller]")] - Этот атрибут указывает, что маршруты к действиям контроллера будут начинаться с имени контроллера в URL. 
    public class ListingController : ControllerBase
    {
        private readonly IProductRepository _repository; 
        private readonly IEnumerable<IProductsSiteClient> _clients; 

        public ListingController(IProductRepository repository, IEnumerable<IProductsSiteClient> clients)
        {
            _repository = repository;
            _clients = clients;
        }

        [HttpGet] 
        [Route("products/")] //Параметры в фигурных скобках ({category:categoryEnum} и {source}) будут извлечены из URL запроса. 
        public async Task<IEnumerable<Product>> GetProducts()// публичный метод  GetProducts с аргументами(Section- тип аргумента, section- название аргумента)  тип  возвращаеммого значение Task<IEnumerable<Product>> 
        {
            var section = Section.Makeup;
            var allListings = new List<Product>();
            for (var pagenumber = 1; pagenumber < 4; pagenumber++)
            {
                foreach (var client in _clients)
                {
                    var products = await client.FetchFromSectionAndPage(section, pagenumber);
                    allListings.AddRange(products);
                }
            }
            await _repository.UpsertManyProducts(allListings);
            return allListings; // Объединяет две последовательности, сохраняя все элементы исходных последовательностей, включая дубликаты.
        } 
    }
}


