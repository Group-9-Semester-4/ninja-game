using System;
using System.Collections;
using System.Threading.Tasks;
using API;
using API.Models;
using API.Params;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public Text GameTimeInput;
    public Toggle MultiplayerToggle;
    public Text LobbyCode;
    public Text UserName;
    
    public GameObject multiplayerInfo;

    private void Start()
    {
        var socketIO = SocketClient.Init("game");
        socketIO.ConnectAsync();
    }

    public void onMultiplayerToggle()
    {
        multiplayerInfo.SetActive(MultiplayerToggle.isOn);
    }

    public void onContinue()
    {
        var inputValue = GameTimeInput.text;

        if (int.TryParse(inputValue, out var gameTime))
        {
            var gameOptions = new GameInitParam();
            
            // We need seconds, so multiply by 60
            gameOptions.timeLimit = gameTime * 60;
            gameOptions.multiPlayer = MultiplayerToggle.isOn;
            gameOptions.lobbyCode = LobbyCode.text;

            StartCoroutine(APIClient.Instance.InitGame(gameOptions, resource =>
            {
                if (gameOptions.multiPlayer)
                {
                    ConnectToLobby(resource);
                }
                else
                {
                    SceneManager.LoadScene("DiscardScene");
                }
            }));
        }
        else
        {
            
            // TODO: Some error message
            Debug.Log("Nothing happened");
        }
    }

    private void ConnectToLobby(API.Models.Game game)
    {
        var param = new JoinGameParam() {lobbyCode = game.gameInfo.lobbyCode, userName = UserName.text};

        var task = SocketClient.Client.EmitAsync("join", ack =>
        {
            var gameInfo = ack.GetValue<GameInfoMessage>().data;
            GameData.Reinstantiate.GameInfo = gameInfo;
        }, param);

        StartCoroutine(ConnectToLobbyRoutine(task));
    }

    private IEnumerator ConnectToLobbyRoutine(IAsyncResult task)
    {
        while (!task.IsCompleted)
        {
            yield return 0;
        }
        
        SceneManager.LoadScene("Lobby");
    }

}
