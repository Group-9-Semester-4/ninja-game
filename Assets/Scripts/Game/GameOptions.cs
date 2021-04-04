using API;
using API.Params;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public Text GameTimeInput;
    public Toggle MultiplayerToggle;

    public void onContinue()
    {
        var inputValue = GameTimeInput.text;

        if (int.TryParse(inputValue, out var gameTime))
        {
            var gameOptions = new GameInitParam();
            
            // We need seconds, so multiply by 60
            gameOptions.timeLimit = gameTime * 60;
            gameOptions.multiPlayer = MultiplayerToggle.isOn;

            StartCoroutine(APIClient.Instance.InitGame(gameOptions, resource =>
            {
                if (gameOptions.multiPlayer)
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/Lobby");
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

}
