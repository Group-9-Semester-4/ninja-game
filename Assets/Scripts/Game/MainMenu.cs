using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject canvas;

    public void OnStart()
    {
        SceneManager.LoadScene("DiscardScene");
    }

    public void OnMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerMainMenu");
    }

    public void OpenPopup()
    {
        canvas.SetActive(true);
    }
}
