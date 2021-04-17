using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public Text ScoreText;

    public GameObject drawCardButton;
    public GameObject startBossFight;
    
    void Start()
    {
        var points = GameData.Instance.Points;

        ScoreText.text = points.ToString();

        if (points > 0)
        {
            startBossFight.SetActive(true);
        }

        if (GameService.Instance.remainingCards().Count == 0)
        {
            drawCardButton.SetActive(false);
        }
    }

    public void DrawCard()
    {
        var card = GameService.Instance.DrawCard();
        
        GameData.Instance.CurrentCard = card;

        if (card != null)
        {
            SceneManager.LoadScene("DrawnCardScene");
        }
    }

    public void BossFight()
    {
        SceneManager.LoadScene("BossScene");
    }

    public void EndGame()
    {
        SceneManager.LoadScene("FinishScene");
    }
}