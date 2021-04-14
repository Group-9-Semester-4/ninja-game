using API;
using API.Params;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void OnStart()
    {
        var gameOptions = new GameInitParam();
        
        gameOptions.timeLimit = 3600;
        gameOptions.multiPlayer = false;
        gameOptions.lobbyCode = "";

        StartCoroutine(APIClient.Instance.InitGame(gameOptions, resource =>
        {
            SceneManager.LoadScene("DiscardScene");
        }));
    }

    public void OnMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerMainMenu");
    }
}
