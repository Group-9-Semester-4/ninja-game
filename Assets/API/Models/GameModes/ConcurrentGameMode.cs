using System;
using System.Collections.Generic;

namespace API.Models.GameModes
{
    
    [Serializable]
    public class ConcurrentGameMode
    {
        public int gameTime;
        public List<Player> players;

        public Dictionary<string, int> playerScores;
        
        public Dictionary<string, int> numberOfPlayerCardsDone;

        public Dictionary<string, int> bossFightScores;
    }
    
    [Serializable]
    public class ConcurrentGameModeGameInfo : GameInfo
    {
        public new ConcurrentGameMode gameModeData;
        
        public new object GameModeData()
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