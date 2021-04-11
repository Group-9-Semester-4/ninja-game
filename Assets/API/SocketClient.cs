using SocketIOClient;
namespace API
{
    public class SocketClient
    {
        public static SocketIO Client;

        public static SocketIO Init(string namespaceUrl)
        {
            return new SocketIO($"http://localhost:8081/{namespaceUrl}");
        }
    }
}