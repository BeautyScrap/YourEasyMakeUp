using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourEasyRent.Entities;

namespace YourEasyRent.UserState
{

    public class UserSearchStateDTO 
    { 
        public string? Id { get; set; }
        public string UserId { get; set; }
        public string ChatId { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public MenuStatus? Menu_status { get; set; }
        //public List<MenuStatus> HistoryOfMenuStatuses { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }

    }


}
