using System;
using System.Collections.Generic;

namespace API.Models
{
    [Serializable]
    public class GameInfo
    {
        public string gameId;
        public List<Card> remainingCards;
        public List<Player> players;
        public string lobbyCode;
        public bool started;
        public string lobbyOwnerId;

    }
}