using MongoDB.Bson;

namespace YourEasyRent.Services.State
{
    public class UserSearchState :IUserSearchState                                             
    {
        public  long UserId { get; private set; }
        public long _chatId { get; private set; }
        public string _category { get; private set; }
        public string _brand{ get; private set; }
        public MenuStatus _menuStatus { get; private set; }

        private List<MenuStatus> menuStatuses = new List<MenuStatus>();

        // steps[start,brand,category] нужно сделать лист или стопку, куда будут записываться статусы бота
        public UserSearchState(long userId)
        {
            UserId = userId;
            _menuStatus = MenuStatus.Started;
        }

        public UserSearchState(UserSearchStateDTO dto)
        {
            UserId = dto.UserId;   
            _chatId = dto.ChatId;
            _category = dto.Category;
            _brand = dto.Brand;
            _menuStatus = dto.Status;
        }

        public void SetBrand(string brand)
        {
            _brand = brand;
            _menuStatus = MenuStatus.BrandChosen;
            menuStatuses.Add(MenuStatus.BrandChosen);   
        }

        public void SetCategory(string category)
        {
            _category = category;
            _menuStatus = MenuStatus.CategoryChosen;
            menuStatuses.Add(MenuStatus.CategoryChosen);
        }

        public void BackOnPreviousStep()
        {
            throw new NotImplementedException();
        }

        public void GetNextMenu() 
        {
            throw new NotImplementedException(); // от того какой будет следующий статус в основном классе TCH зависит показ следующего меню
            // те мы передаем сюда аргумент со статусом , который потом будем сопоставлять с словарем и присылать в ответ нужное меню
        }

        public void IsFiniShed()//
        {
            throw new NotImplementedException();
        }

        public UserSearchStateDTO TOMongoRepresintation()
        {
            var userSearchStateDTO = new UserSearchStateDTO()
            {
                UserId = UserId,
                ChatId = _chatId,
                Brand = _brand,
                Category = _category,
                Status = _menuStatus,
            };
            return userSearchStateDTO;
        }

        
    }
}
