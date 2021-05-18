using API;
using API.Models;
using API.Params;
using Game;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnitySocketIO;

public class JoinGame : MonoBehaviour
{
    public Text userName;
    public Text lobbyCode;
    
    private SocketIOController _socketIO;
    private bool connect;
    
    void Start()
    {
        _socketIO = SocketClient.Client;
    }

    public void Connect()
    {
        var param = new JoinGameParam() {lobbyCode = lobbyCode.text, userName = userName.text};

        _socketIO.Emit("join", JsonUtility.ToJson(param), response =>
        {
            var message = JsonConvert.DeserializeObject<GameInfoMessage>(response);

            if (message.IsSuccess())
            {
                GameData.Reinstantiate.GameInfo = message.data;
                connect = true;
            }
        });

    }

    private void Update()
    {
        if (connect)
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
