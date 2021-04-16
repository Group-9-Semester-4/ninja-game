using Game;
using UnityEngine.SceneManagement;

public class DiscardScene : DiscardCardsScript
{
    void Start()
    {
        base.Start();
    }

    public void Continue()
    {
        GameService.Instance.StartGame(getCards());
        
        SceneManager.LoadScene("GameScene");
    }
}
