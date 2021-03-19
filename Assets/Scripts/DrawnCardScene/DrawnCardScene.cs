using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawnCardScene : MonoBehaviour
{
    public void CompleteCard()
    {
        var gameData = GameData.Instance;

        if (gameData.CurrentCard != null)
        {
            gameData.Points += gameData.CurrentCard.points;
            gameData.CurrentCard = null;
        }

        SceneManager.LoadScene("GameScene");
    }
}
