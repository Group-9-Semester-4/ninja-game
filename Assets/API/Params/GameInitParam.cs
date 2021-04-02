using System;

namespace API.Params
{
    [Serializable]
    public class GameInitParam
    {
        public int timeLimit = 3600;
        public bool singlePlayer = true;
        public bool playingAlone = true;
    }
}