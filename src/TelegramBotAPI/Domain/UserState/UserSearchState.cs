using YourEasyRent.Entities;

namespace YourEasyRent.UserState
{
    public class UserSearchState
    {
        public string UserId { get; private set; } 
        public string? ChatId { get; private set; }
        public string? Category { get; private set; }// AK TODO  чекнуть что эти поля в базе могут быть null, тк они заполняются постепенно
        public string? Brand { get; private set; }
        public string? Name { get; private set; }
        public decimal? Price { get; private set; }
        public MenuStatus MenuStatus { get; private set; }
        public bool IsFinished => IsReadyForSearch();
  
        public UserSearchState()
        {
        }

        public static UserSearchState CreateNewUserSearchState(string userId) 
        {
            UserSearchState userSearchState = new UserSearchState
            {
                UserId = userId,
                //HistoryOfMenuStatuses = new List<MenuStatus>(),
                MenuStatus = MenuStatus.Started,
   
            };
            return userSearchState;
        }
        public void SetProductNameAndPrice(string productName, decimal pdroductPrice)
        {
            Name = productName;
            Price = pdroductPrice;
        }

        public void SetChatId(string chatId)
        {
            ChatId = chatId;
        }
        public void SetBrand(string brand)
        {
            Brand = brand;
            MenuStatus = MenuStatus.BrandChosen;
        }

        public void SetCategory(string category)
        {
            Category = category;
            MenuStatus = MenuStatus.CategoryChosen;
        }

        public void AddStatusToHistory(MenuStatus status)
        {
            MenuStatus = status;
        }
        public void GetNextMenu()
        {
            throw new NotImplementedException(); // от того какой будет следующий статус в основном классе TCH зависит показ следующего меню
            // те мы передаем сюда аргумент со статусом , который потом будем сопоставлять с словарем и присылать в ответ нужное меню
        }

        public bool IsReadyForSearch()
        {
            if (UserId == null || Brand == null || Category == null)
            {
                return false;
            }
            return true;
        }

        public  UserSearchStateDTO ToDto()
        { 
            var userSearchStateDTO = new UserSearchStateDTO()
            {
                UserId = UserId,
                ChatId = ChatId,
                Brand = Brand,
                Category = Category,
                Menu_status = MenuStatus,
                Name = Name,
                Price = Price ?? 0
            };
            return userSearchStateDTO;
        }

        public static UserSearchState FromDto(string userId, UserSearchStateDTO dto)
        {
            UserSearchState userSearchState = new UserSearchState()
            {
                UserId = userId,
                ChatId = dto.ChatId,
                Brand = dto.Brand,
                Category = dto.Category,
                MenuStatus = dto.Menu_status.Value,
                Name = dto.Name,
                Price = dto.Price,
            };
            return userSearchState;
        }



    }
}


