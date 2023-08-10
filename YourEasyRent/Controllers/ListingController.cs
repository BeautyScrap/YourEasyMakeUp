using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types.Payments;
using HtmlAgilityPack;
using YourEasyRent.Entities;
using YourEasyRent.Services;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;

namespace YourEasyRent.Controllers

{

    [ApiController]
    [Route("[controller]")]
    public class ListingController : Controller
    {
        [HttpGet(Name = "Search")]
        public async Task<IEnumerable<Product>> Get(Entities.Section section)
        {
            
            var sephoraClient = new SephoraClient();
            var sephoralistings = await sephoraClient.FetchFromSection(section); //  сделать 2 listinga для сефоры и для дугласа, смерджить и результаты(list или join  погуглить как) и вернуть общий результат для обоих сайтов 
                       
            var douglasClient = new DouglasClient(); // сделать цикл с перебором страниц, чтобы "перелистывать страницы" и брать новые ответы

            var douglaslistings = await douglasClient.FetchFromSection(section, 1);
            var allListings = sephoralistings.Concat(douglaslistings); 
            // Объединяет две последовательности, сохраняя все элементы исходных последовательностей, включая дубликаты. 
            return allListings;




        }




    }   
}


