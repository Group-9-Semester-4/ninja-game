using System;

namespace API.Models
{
    [Serializable]
    public class GameInfoMessage : SocketIOMessage
    {
        public GameInfo data;
    }
}