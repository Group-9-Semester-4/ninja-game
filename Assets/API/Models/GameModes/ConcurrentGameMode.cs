using System;
using System.Collections.Generic;
using API.Models.HelperModels;

namespace API.Models.GameModes
{
    
    [Serializable]
    public class ConcurrentGameMode
    {
        public int gameTime;
        public List<Player> players;

        public List<PlayerScore> playerScores;
        
        public List<CardsDone> numberOfPlayerCardsDone;

        public List<BossFightScore> bossFightScores;
        
        public int GetScore(string playerId)
        {
            var playerScore = playerScores.Find(score => score.playerId == playerId);
            
            return playerScore != null ? playerScore.score : 0;
        }

        public int NumberOfCardsDone(string playerId)
        {
            var cardsDone = numberOfPlayerCardsDone.Find(done => done.playerId == playerId);

            return cardsDone != null ? cardsDone.cardsDone : 0;
        }
    }
    
    [Serializable]
    public class ConcurrentGameModeGameInfo : GameInfo
    {
        public ConcurrentGameMode gameModeData;
        
        public new ConcurrentGameMode GameModeData()
        {
            return gameModeData;
        }
    }
    
    [Serializable]
    public class ConcurrentGameModeGameInfoMessage : GameInfoMessage
    {
        public new ConcurrentGameModeGameInfo data;
        
        public new ConcurrentGameModeGameInfo GameInfo()
        {
            return data;
        }
    }
}