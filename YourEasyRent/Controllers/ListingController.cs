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
using Microsoft.AspNetCore.SignalR.Protocol;
using YourEasyRent.DataBase;

namespace YourEasyRent.Controllers

{

    [ApiController]
    [Route("[controller]")] // было [Route("api/v1/[controller]")] - Этот атрибут указывает, что маршруты к действиям контроллера будут начинаться с имени контроллера в URL. 
    public class ListingController : ControllerBase
    {
        private readonly IProductRepository _repository; // объявляет приватное поле _repository в текущем классе ListingController, которое будет использоваться для внедрения зависимости  IProductRepository. Это поле будет доступно для использования в методах и конструкторах класса
        private readonly IDouglasProductSiteClient _dclient;
        private readonly ISephoraProductSiteClient _sclient;

        public ListingController(IProductRepository repository, IDouglasProductSiteClient dclient, ISephoraProductSiteClient sclient)//  конструктор
        {
            _repository = repository;
            _dclient = dclient; 
            _sclient = sclient;
        }




        [HttpGet] // было [HttpGet(Name = "Search")]
        [Route("products/")] //Параметры в фигурных скобках ({category:categoryEnum} и {source}) будут извлечены из URL запроса. 
        public async Task<IEnumerable<Product>> GetProducts()// публичный метод  GetProducts с аргументами(Section- тип аргумента, section- название аргумента)  тип  возвращаеммого значение Task<IEnumerable<Product>> 
        {
            var section = Section.Makeup;

            var pagenumber = 1;
            var sephoralistings = await _sclient.FetchFromSephoraSection(section, pagenumber); //  сделать 2 listinga для сефоры и для дугласа, смерджить и результаты(list или join  погуглить как) и вернуть общий результат для обоих сайтов 

             // сделать цикл с перебором страниц, чтобы "перелистывать страницы" и брать новые ответы

            var douglaslistings = await _dclient.FetchFromDouglasSection(section, pagenumber);
            var allListings = sephoralistings.Concat(douglaslistings); 
            // Объединяет две последовательности, сохраняя все элементы исходных последовательностей, включая дубликаты.
            await _repository.CreateMany(allListings);// можно заменить на метод upsert, который будет обновлять данные  продуктах, которые дублировуются
            return allListings;


        }




    }   
}


