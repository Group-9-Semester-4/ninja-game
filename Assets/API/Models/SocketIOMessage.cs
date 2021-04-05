using System;

namespace API.Models
{
    [Serializable]
    public class SocketIOMessage
    {
        public string type;
        public string reason;
    }
}