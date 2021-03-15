using System;

namespace APIClient.Models
{
    [Serializable]
    public class GameInitOptions
    {
        public int timeLimit = 3600;
        public bool singlePlayer = true;
        public bool playingAlone = true;
    }
}