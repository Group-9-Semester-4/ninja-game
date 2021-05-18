using System;

namespace API.Models
{
    [Serializable]
    public class DrawCardMessage : SocketIOMessage
    {
        public Card data;
    }
}