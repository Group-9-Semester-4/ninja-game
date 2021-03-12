using Models;

namespace APIClient
{
    public class APIClient
    {
        // TODO: Change to dynamic env
        protected string APIUrl = "http://localhost:8080";

        private static APIClient _instance;
    
        private APIClient() {}

        public static APIClient Instance => _instance ?? (_instance = new APIClient());

        public CardAPIResource DrawCard()
        {
            // TODO: Change to actual API loading
        
            var card = new CardAPIResource()
            {
                id = 1, 
                imageUrl = "https://i.pinimg.com/originals/9f/ce/f1/9fcef1014d0d405429dfd38a4bc7aeba.jpg"
            };

            return card;
        }
    }
}
