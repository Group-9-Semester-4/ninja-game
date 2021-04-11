using API;
using UnityEngine.SceneManagement;

public class DiscardScene : DiscardCardsScript
{
    void Start()
    {
        base.Start();
    }

    public void Continue()
    {
        var options = getGameStartParam();

        StartCoroutine(APIClient.Instance.StartGame(options, resource =>
        {
            SceneManager.LoadScene("GameScene");
        }));

    }
}
