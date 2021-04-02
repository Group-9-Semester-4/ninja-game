using APIClient.Params;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public Text GameTimeInput;

    public void onContinue()
    {
        var inputValue = GameTimeInput.text;

        if (int.TryParse(inputValue, out var gameTime))
        {
            var gameOptions = new GameInitParam();
            
            // We need seconds, so multiply by 60
            gameOptions.timeLimit = gameTime * 60;

            StartCoroutine(APIClient.APIClient.Instance.InitGame(gameOptions, resource =>
            {
                SceneManager.LoadScene("DiscardScene");
            }));
        }
        else
        {
            
            // TODO: Some error message
            Debug.Log("Nothing happened");
        }
    }

}
