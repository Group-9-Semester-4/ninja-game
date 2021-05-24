using API.Models;
using API.Models.GameModes;
using UnityEngine;

namespace Game
{
    public class Helper
    {
        public static IGameInfo DeserializeGameInfo(string jsonData)
        {
            var gameInfo = JsonUtility.FromJson<GameInfo>(jsonData);

            return gameInfo.gameModeId switch
            {
                "basic" => JsonUtility.FromJson<BasicGameModeGameInfo>(jsonData),
                "concurrent" => JsonUtility.FromJson<ConcurrentGameModeGameInfo>(jsonData),
                "deathmatch arena" => JsonUtility.FromJson<DeathmatchGameModeGameInfo>(jsonData),
                _ => gameInfo
            };
        }
        
        public static IGameInfoMessage DeserializeGameInfoMessage(string jsonData)
        {
            var gameInfoMessage = JsonUtility.FromJson<GameInfoMessage>(jsonData);

            return gameInfoMessage.GameInfo().GameModeId() switch
            {
                "basic" => JsonUtility.FromJson<BasicGameModeGameInfoMessage>(jsonData),
                "concurrent" => JsonUtility.FromJson<ConcurrentGameModeGameInfoMessage>(jsonData),
                "deathmatch arena" => JsonUtility.FromJson<DeathmatchGameModeGameInfoMessage>(jsonData),
                _ => gameInfoMessage
            };
        }
    }
}