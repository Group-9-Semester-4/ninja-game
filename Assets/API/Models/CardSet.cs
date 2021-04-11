using System;
using System.Collections.Generic;

namespace API.Models
{
    [Serializable]
    public class CardSet
    {
        public string id;
        public string name;
        public int completeTimeLimit;
        public int difficulty;
        public bool multiplayerSuitable;
        public bool temporary;
        public List<Card> cards;
    }
}