using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types.Payments;
using HtmlAgilityPack;
using YourEasyRent.Entities;
using YourEasyRent.Services;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;
using YourEasyRent.DataBase.Interfaces;
using Section = YourEasyRent.Entities.Section;

namespace YourEasyRent.Controllers

{

    [ApiController]
    [Route("[controller]")] // было [Route("api/v1/[controller]")] - Этот атрибут указывает, что маршруты к действиям контроллера будут начинаться с имени контроллера в URL. 
    public class ListingController : ControllerBase

    {
        private readonly IProductRepository _repository;

        public ListingController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet] // было [HttpGet(Name = "Search")]
        [Route("products/")] //Параметры в фигурных скобках ({category:categoryEnum} и {source}) будут извлечены из URL запроса. 
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var section = Section.Makeup;
            var sephoraClient = new SephoraClient();
            var sephoralistings = await sephoraClient.FetchFromSection(section,0); //  сделать 2 listinga для сефоры и для дугласа, смерджить и результаты(list или join  погуглить как) и вернуть общий результат для обоих сайтов 
                       
            var douglasClient = new DouglasClient(); // сделать цикл с перебором страниц, чтобы "перелистывать страницы" и брать новые ответы

            var douglaslistings = await douglasClient.FetchFromSection(section, 1);
            var allListings = sephoralistings.Concat(douglaslistings); 
            // Объединяет две последовательности, сохраняя все элементы исходных последовательностей, включая дубликаты.
            await _repository.CreateMany(allListings);
            return allListings;




        }




    }   
}


