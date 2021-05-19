using System;

namespace API.Models
{
    [Serializable]
    public class GameInfoMessage : SocketIOMessage, IGameInfoMessage
    {
        public GameInfo data;

        public IGameInfo GameInfo()
        {
            return data;
        }
    }

    public interface IGameInfoMessage : ISocketIOMessage
    {
        public IGameInfo GameInfo();
    }
}