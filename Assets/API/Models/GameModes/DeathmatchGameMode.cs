using System;
using System.Collections.Generic;
using API.Models.HelperModels;

namespace API.Models.GameModes
{
    
    [Serializable]
    public class DeathmatchGameMode
    {
        public List<Player> players;

        public List<PlayerScore> playerScores;
        
        public List<CardLockInfo> remainingCards;

        public List<string> playersReady;
        
        public int GetScore(string playerId)
        {
            var playerScore = playerScores.Find(score => score.playerId == playerId);
            
            return playerScore != null ? playerScore.score : 0;
        }
    }
    
    [Serializable]
    public class DeathmatchGameModeGameInfo : GameInfo
    {
        public DeathmatchGameMode gameModeData;
        
        public new DeathmatchGameMode GameModeData()
        {
            return gameModeData;
        }
    }
    
    [Serializable]
    public class DeathmatchGameModeGameInfoMessage : GameInfoMessage
    {
        public new DeathmatchGameModeGameInfo data;
        
        public new DeathmatchGameModeGameInfo GameInfo()
        {
            return data;
        }
    }
}