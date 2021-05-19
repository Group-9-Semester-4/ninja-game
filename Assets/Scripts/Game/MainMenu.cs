using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GameService.Reinstantiate();
    }

    public void OnStart()
    {
        SceneManager.LoadScene("DiscardScene");
    }

    public void OnMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerMainMenu");
    }
}
