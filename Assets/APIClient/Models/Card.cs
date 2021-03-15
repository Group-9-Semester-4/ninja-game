using System;

namespace APIClient.Models
{
    [Serializable]
    public class Card
    {
        public string id;
        // TODO: Change when API implements this field
        public string imageUrl = "https://i.pinimg.com/originals/9f/ce/f1/9fcef1014d0d405429dfd38a4bc7aeba.jpg";
        public string name;
        public string description;
        public int points;
    }
}