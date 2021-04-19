using API.Models;

namespace Game
{
    public class GameData
    {
        // Singleton stuff
        private static GameData _instance;

        private GameData()
        {
            Points = 0;
        }

        public static GameData Instance => _instance ?? (_instance = new GameData());

        public static GameData Reinstantiate => (_instance = new GameData());

        // Fields

        public int Points { get; set; }

        public Card CurrentCard { get; set; }
        
        public bool IsMultiplayer { get; set; }

        public GameInfo GameInfo { get; set; }
    }
}