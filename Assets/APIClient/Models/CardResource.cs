using System;

namespace APIClient.Models
{
    [Serializable]
    public class CardResource
    {
        public string id;
        public string name;
        public string description;
        public int points;
        public bool difficulty_type;
        public bool difficulty;
        public string filepath;
    }
}