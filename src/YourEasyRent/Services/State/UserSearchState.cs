namespace YourEasyRent.Services.State
{
    public class UserSearchState : IUserSearchState
    {
        private long _userId;
        private long _chatId;
        private string _category;
        private string _brand;
        private MenuStatus _menuStatus;

        public UserSearchState(long userId,long chatId, string category, string brand, MenuStatus menuStatus )
        {
            _userId = userId;   
            _chatId = chatId;
            _category = category;
            _brand = brand;
            _menuStatus = menuStatus;

        }


        public void StartSearch(long chatId)
        {
            var chatIdNew = _chatId;
        }
        public void SetBrand(string brand)
        {
            throw new NotImplementedException();
        }

        public void SetCategory(string category)
        {
            throw new NotImplementedException();
        }

        public void BackOnPreviousStep()
        {
            throw new NotImplementedException();
        }

        public void GetNextMenu()
        {
            throw new NotImplementedException();
        }

        public void IsFiniShed()
        {
            throw new NotImplementedException();
        }


        public Task TOMongoRepresintation()
        {
            throw new NotImplementedException();
        }
    }
}
