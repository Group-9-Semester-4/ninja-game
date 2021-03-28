using APIClient.Models;
using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public Text GameTimeInput;

    public void onContinue()
    {
        var inputValue = GameTimeInput.text;

        if (int.TryParse(inputValue, out var gameTime))
        {
            var gameOptions = new GameInitOptions();
            
            // We need seconds, so multiply by 60
            gameOptions.timeLimit = gameTime * 60;

            //APIClient.APIClient.Instance.InitGame(gameOptions);

            StartCoroutine(APIClient.APIClient.Instance.InitGame(gameOptions));
        }
        else
        {
            
            // TODO: Some error message
            Debug.Log("Nothing happened");
        }
    }

}
