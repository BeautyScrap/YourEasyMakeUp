using Microsoft.AspNetCore.Mvc;
using YourEasyRent.DataBase;

namespace YourEasyRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MongoController : Controller
    {
        private readonly MongoCollection _mongoCollection;
        public MongoController(MongoCollection mongoCollection) => _mongoCollection = mongoCollection;  
       
    }
}
