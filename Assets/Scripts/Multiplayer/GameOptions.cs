using API;
using API.Models;
using API.Params;
using Game;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public Text GameTimeInput;
    public Text LobbyCode;
    public Text UserName;

    public bool joinLobby;

    private void Update()
    {
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
    }

    private void ConnectToLobby(API.Models.Game game)
    {
        var param = new JoinGameParam() {lobbyCode = game.gameInfo.lobby.lobbyCode, userName = UserName.text};

        SocketClient.Client.Emit("join", JsonUtility.ToJson(param), ack =>
        {
            var message = Helper.DeserializeGameInfoMessage(ack);
            if (message.IsSuccess())
            {
                var gameInfo = message.GameInfo();
                GameData.Reinstantiate.GameInfo = gameInfo;
                joinLobby = true;
            }
        });

    }

}
