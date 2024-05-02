using MongoDB.Bson;
using Telegram.Bot.Types;
using YourEasyRent.Entities;

namespace YourEasyRent.UserState
{
    public class UserSearchState
    {
        public string UserId { get; private set; }
        public string ChatId { get; private set; }
        public string Category { get; private set; }
        public string Brand { get; private set; }
        public MenuStatus CurrentMenuStatus { get; private set; }
        public IEnumerable<MenuStatus> HistoryOfMenuStatuses
        {
            get { return _historyOfMenuStatuses; } 
            private set { _historyOfMenuStatuses = (List<MenuStatus>)value; }
        }

        private List<MenuStatus> _historyOfMenuStatuses = new List<MenuStatus>();

        public UserSearchState(string userId)
        {
            UserId = userId.ToString();
            CurrentMenuStatus = MenuStatus.Started;
            _historyOfMenuStatuses.Add(MenuStatus.Started);

        }

        public UserSearchState(UserSearchStateDTO dto)
        {
            UserId = dto.UserId;
            ChatId = dto.ChatId;
            Category = dto.Category;
            Brand = dto.Brand;
            CurrentMenuStatus = dto.Status;

        }

        //public void SetStatus(MenuStatus status)
        //{
        //    _historyOfMenuStatuses.Add(status);
        //}
        public void SetChatId(string chatId)
        {
            ChatId = chatId;
        }
        public void SetBrand(string brand)
        {
            Brand = brand;
            CurrentMenuStatus = MenuStatus.BrandChosen;
            _historyOfMenuStatuses.Add(MenuStatus.BrandChosen);
        }

        public void SetCategory(string category)
        {
            Category = category;
            CurrentMenuStatus = MenuStatus.CategoryChosen;
            _historyOfMenuStatuses.Add(MenuStatus.CategoryChosen);
        }

        public void AddStatusToHistory(MenuStatus status)
        {
            _historyOfMenuStatuses.Add(status);
        }
        //public void SetCurrentState(MenuStatus status)
        //{
        //    var currentState = MenuStatus(status);
        //}

        public void BackOnPreviousStep(MenuStatus status)
        {
            _historyOfMenuStatuses.Last();
            _historyOfMenuStatuses.Remove(status);
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
                Status = CurrentMenuStatus,
            };
            return userSearchStateDTO;
        }

    }
}
