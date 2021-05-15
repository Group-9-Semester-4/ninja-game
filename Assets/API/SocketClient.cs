using SocketIOClient;
namespace API
{
    public class SocketClient
    {
        public static SocketIO Client;

        public static SocketIO Init()
        {
            Client = new SocketIO($"http://localhost:8081");
            return Client;
        }
    }
}