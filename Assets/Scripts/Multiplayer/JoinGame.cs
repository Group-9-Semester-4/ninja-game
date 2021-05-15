using API;
using API.Models;
using API.Params;
using Game;
using SocketIOClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinGame : MonoBehaviour
{
    public Text userName;
    public Text lobbyCode;
    
    private SocketIO _socketIO;
    private bool connect;
    
    void Start()
    {
        _socketIO = SocketClient.Init();

        _socketIO.ConnectAsync();
    }

    public void Connect()
    {
        var param = new JoinGameParam() {lobbyCode = lobbyCode.text, userName = userName.text};

        var task = _socketIO.EmitAsync("join", response =>
        {
            var message = response.GetValue<GameInfoMessage>();

            if (message.IsSuccess())
            {
                GameData.Reinstantiate.GameInfo = message.data;
                connect = true;
            }

        }, param);

    }

    private void Update()
    {
        if (connect)
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
