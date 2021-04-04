using System;

namespace API.Models
{
    [Serializable]
    public class Card
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