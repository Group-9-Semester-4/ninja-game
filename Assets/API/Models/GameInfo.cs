using System;
using System.Collections.Generic;

namespace API.Models
{
    [Serializable]
    public class GameInfo
    {
        public string gameId;
        public bool started;
        public Lobby lobby;
        public object gameModeData;

    }
}