using System;

namespace APIClient.Models
{
    [Serializable]
    public class Card
    {
        public string id;
        public string imageUrl;
        public string name;
        public string description;
        public int points;
    }
}