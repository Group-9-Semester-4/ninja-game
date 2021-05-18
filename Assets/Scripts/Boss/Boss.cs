using System.Collections;
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
        GameData.Instance.Points = 0;
        if (GameData.Instance.IsMultiplayer)
        {
            SendResult();
            return;
        }

        GameService.Instance.score = bossScore.score;

        var finishParam = GameService.Instance.FinishGameParam();

        StartCoroutine(APIClient.Instance.FinishGame(finishParam, () =>
        {
            SceneManager.LoadScene("FinishScene");
        }));
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
                case "concurrent":
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/Concurrent/AfterConcurrentBoss");
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
                socketIo.Emit("basic.boss-complete", JsonUtility.ToJson(bossScoreParam), response =>
                {
                    var message = JsonUtility.FromJson<GameInfoMessage>(response);

                    if (message.IsSuccess())
                    {
                        GameData.Instance.GameInfo = message.data;
                        multiplayerMoveOn = true;
                    }
                
                });
                break;
            }
            case "concurrent":
            {
                socketIo.Emit("concurrent.boss-complete", JsonUtility.ToJson(bossScoreParam), response =>
                {
                    var message = JsonUtility.FromJson<GameInfoMessage>(response);

                    if (message.IsSuccess())
                    {
                        GameData.Instance.GameInfo = message.data;
                        multiplayerMoveOn = true;
                    }
                
                });
                break;
            }
        }
    }

    public void TakeHit()
    {
        StartCoroutine(ColorChangeTime());
    }
    
    
    IEnumerator ColorChangeTime()
    {
        // Change color to red
        GetComponent<Renderer>().material.color = new Color(0.5f, 0f, 0f);
        
        // Wait one second
        yield return new WaitForSecondsRealtime(0.1F);

        // change color back to normal
        GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
    }
    
}

