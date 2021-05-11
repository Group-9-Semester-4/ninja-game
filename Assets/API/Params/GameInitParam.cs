using System;

namespace API.Params
{
    [Serializable]
    public class GameInitParam
    {
        public int timeLimit = 3600;
        public bool multiPlayer = false;
        public bool playingAlone = true;
        public string lobbyCode;
        public string email = "test@test.com";
    }
}