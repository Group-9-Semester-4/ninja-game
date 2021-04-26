using System.Collections.Generic;

namespace API.Models.GameModes
{
    public class ConcurrentGameMode
    {
        public int gameTime;
        public List<Player> players;

        public Dictionary<string, int> playerScores;
        
        public Dictionary<string, int> numberOfPlayerCardsDone;
    }
}