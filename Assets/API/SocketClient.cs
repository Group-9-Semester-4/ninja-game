using SocketIOClient;
namespace API
{
    public class SocketClient
    {
        public static SocketIO Client;

        public static SocketIO Init(string namespaceUrl)
        {
            Client = new SocketIO($"http://localhost:8081/{namespaceUrl}");
            return Client;
        }
    }
}