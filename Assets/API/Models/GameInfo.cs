using System;
using Newtonsoft.Json.Linq;

namespace API.Models
{
    [Serializable]
    public class GameInfo
    {
        public string gameId;
        public bool started;
        public Lobby lobby;
        public JObject gameModeData;

    }
}