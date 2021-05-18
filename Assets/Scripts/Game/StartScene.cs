using System.Text.RegularExpressions;
using API;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnitySocketIO;

public class StartScene : MonoBehaviour
{
    public  const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
    
    public InputField emailInput;
    public GameObject errorMessage;

    public SocketIOController socketIOController;

    private void Start()
    {
        SocketClient.Client = socketIOController;
        socketIOController.Connect();
    }

    public void OnContinue()
    {
        var email = emailInput.text;

        if (!string.IsNullOrEmpty(email) && Regex.IsMatch(email, MatchEmailPattern))
        {
            GameService.playerEmail = email;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
}
