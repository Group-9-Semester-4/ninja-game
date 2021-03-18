using System;
using System.Collections.Generic;

namespace APIClient.Models
{
    [Serializable]
    public class GameResource
    {
        public string uuid;
        public List<CardResource> allCards;
        public int points;
        public int miniGameAttempts;
        public int cardsDone;
        public int timeLimit;
        public bool singlePlayer;
        public bool playingAlone;
    }
}