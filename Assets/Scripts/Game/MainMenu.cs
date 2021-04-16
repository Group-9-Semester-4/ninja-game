using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void OnStart()
    {
        SceneManager.LoadScene("DiscardScene");
    }

    public void OnMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerMainMenu");
    }
}
