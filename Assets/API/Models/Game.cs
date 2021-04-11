using System;
using System.Collections.Generic;

namespace API.Models
{
    [Serializable]
    public class Game
    {
        public string id;
        public int points;
        public int miniGameAttempts;
        public int gameAttempts;
        public int cardsDone;
        public int timeLimit;
        public bool multiPlayer;
        public bool playingAlone;
        public GameInfo gameInfo;
    }
}