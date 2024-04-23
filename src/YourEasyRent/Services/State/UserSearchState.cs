using MongoDB.Bson;

namespace YourEasyRent.Services.State
{
    public class UserSearchState                                            
    {
        public  long UserId { get; set; }
        public long ChatId { get; private set; }
        public string Category { get; private set; }
        public string Brand{ get; private set; }
        public MenuStatus _menuStatus { get; private set; }

        private List<MenuStatus> menuStatuses = new List<MenuStatus>();

        public UserSearchState(long userId)
        {
            UserId = userId;
            _menuStatus = MenuStatus.Started;
        }

        public UserSearchState(UserSearchStateDTO dto)
        {
            UserId = dto.UserId;   
            ChatId = dto.ChatId;
            Category = dto.Category;
            Brand = dto.Brand;
            _menuStatus = dto.Status;

        }

        public void SetStatus(MenuStatus status)
        {
            menuStatuses.Add(status);
        }
        public void SetChatId(long chatId)
        {
            ChatId = chatId;
        }
        public void SetBrand(string brand)
        {
            Brand = brand;
            _menuStatus = MenuStatus.BrandChosen;
            menuStatuses.Add(MenuStatus.BrandChosen);   
        }

        public void SetCategory(string category)
        {
            Category = category;
            _menuStatus = MenuStatus.CategoryChosen;
            menuStatuses.Add(MenuStatus.CategoryChosen);
        }

        public void BackOnPreviousStep(MenuStatus status)
        {
            menuStatuses.Last();
            menuStatuses.Remove(status);
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
                ChatId = ChatId,
                Brand = Brand,
                Category = Category,
                Status = _menuStatus,
            };
            return userSearchStateDTO;
        }

    }
}
