using System;
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
    public Text LobbyCode;
    public Text UserName;

    public bool joinLobby;
    public string result = "none";

    private void Update()
    {
        Debug.Log(result);
        if (joinLobby)
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    public void onContinue()
    {
        var inputValue = GameTimeInput.text;

        if (int.TryParse(inputValue, out var gameTime))
        {
            var gameOptions = new GameInitParam();
            
            // We need seconds, so multiply by 60
            gameOptions.timeLimit = gameTime * 60;
            gameOptions.multiPlayer = true;
            gameOptions.lobbyCode = LobbyCode.text;
            gameOptions.email = GameService.playerEmail;

            StartCoroutine(APIClient.Instance.InitGame(gameOptions, ConnectToLobby));
        }
        else
        {
            
            // TODO: Some error message
            Debug.Log("Nothing happened");
        }
    }

    private void ConnectToLobby(API.Models.Game game)
    {
        var param = new JoinGameParam() {lobbyCode = game.gameInfo.lobby.lobbyCode, userName = UserName.text};

        SocketClient.Client.Emit("join", JsonUtility.ToJson(param), ack =>
        {
            result = ack;
            var message = JsonUtility.FromJson<GameInfoMessage>(ack);
            if (message.IsSuccess())
            {
                var gameInfo = message.data;
                GameData.Reinstantiate.GameInfo = gameInfo;
                joinLobby = true;
            }
        });

    }

}
