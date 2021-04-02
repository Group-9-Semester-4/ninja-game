using System;
using System.Collections.Generic;

namespace API.Models
{
    [Serializable]
    public class GameResource
    {
        public string id;
        public List<CardResource> allCards;
        public int points;
        public int miniGameAttempts;
        public int cardsDone;
        public int timeLimit;
        public bool singlePlayer;
        public bool playingAlone;
    }
}