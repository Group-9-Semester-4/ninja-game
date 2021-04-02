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
        public bool difficultyType;
        public int difficulty;
        public string filepath;
        public string absoluteServerPath;
    }
}