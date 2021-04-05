using System;
using System.Collections.Generic;

namespace API.Models
{
    [Serializable]
    public class Lobby
    {
        public string lobbyCode;
        public string lobbyOwnerId;
        public string gameMode;
        public List<Player> players;
    }
}