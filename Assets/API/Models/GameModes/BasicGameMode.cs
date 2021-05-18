using System;
using System.Collections.Generic;
using API.Models.HelperModels;

namespace API.Models.GameModes
{
    [Serializable]
    public class BasicGameMode
    {
        public string playerOnTurn;
        public Card drawnCard;

        public int score;
        
        public List<Card> remainingCards;
        public List<Player> players;
        
        public List<string> completeStates;
        public List<BossFightScore> bossFightScores;

        public bool HasCompleted(string playerId)
        {
            return completeStates.Contains(playerId);
        }
    }

    [Serializable]
    public class BasicGameModeGameInfo : GameInfo
    {
        public new BasicGameMode gameModeData;
        
        public new object GameModeData()
        {
            return gameModeData;
        }
    }

    [Serializable]
    public class BasicGameModeGameInfoMessage : GameInfoMessage
    {
        public new BasicGameModeGameInfo data;
        
        public new BasicGameModeGameInfo GameInfo()
        {
            return data;
        }
    }
}