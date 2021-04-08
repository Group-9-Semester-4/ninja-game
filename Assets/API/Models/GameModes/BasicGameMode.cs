using System;
using System.Collections.Generic;
using Game;

namespace API.Models.GameModes
{
    [Serializable]
    public class BasicGameMode
    {
        public string playerOnTurn;
        public Card drawnCard;
        
        public List<Card> remainingCards;
        public List<Player> players;
        
        public Dictionary<string, bool> completeStates;
    }
}