using System;

namespace API.Models
{
    [Serializable]
    public class SocketIOMessage : ISocketIOMessage
    {
        public string type;
        public string reason;

        public bool IsSuccess()
        {
            return type == "SUCCESS";
        }
    }

    public interface ISocketIOMessage
    {
        public bool IsSuccess();
    }
}