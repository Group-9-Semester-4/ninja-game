using System;

namespace API.Models
{
    [Serializable]
    public class GameInfo : IGameInfo
    {
        public string gameId;
        public bool started;
        public Lobby lobby;
        public string gameModeId;

        public object GameModeData()
        {
            return null;
        }

        public Lobby Lobby()
        {
            return lobby;
        }

        public string GameModeId()
        {
            return gameModeId;
        }
    }

    public interface IGameInfo
    {
        public object GameModeData();
        public Lobby Lobby();
        public string GameModeId();
    }
}