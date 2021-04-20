using API;
using API.Models;
using API.Params;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public BossScore bossScore;

    public bool multiplayerMoveOn;

    public void FinishGame()
    {
        if (GameData.Instance.IsMultiplayer)
        {
            SendResult();
            return;
        }

        GameService.Instance.score = bossScore.score;
        SceneManager.LoadScene("FinishScene");
    }
    
    public void Update()
    {
        if (multiplayerMoveOn)
        {
            var gameMode = GameData.Instance.GameInfo.gameModeId;
            switch (gameMode)
            {
                case "basic":
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/Basic/AfterBoss");
                    break;
                }
            }
        }
    }
    
    private void SendResult()
    {
        var socketIo = SocketClient.Client;

        var bossScoreParam = new BossScoreParam() { score = bossScore.score };
        
        var gameMode = GameData.Instance.GameInfo.gameModeId;
        switch (gameMode)
        {
            case "basic":
            {
                socketIo.EmitAsync("basic.boss-complete", response =>
                {
                    var message = response.GetValue<GameInfoMessage>();

                    if (message.IsSuccess())
                    {
                        GameData.Instance.GameInfo = message.data;
                        multiplayerMoveOn = true;
                    }
                
                }, bossScoreParam);
                break;
            }
        }
    }
    
}

