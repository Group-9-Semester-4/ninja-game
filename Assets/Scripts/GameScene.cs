using API;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public Text ScoreText;

    void Start()
    {
        var points = GameData.Instance.Points;

        ScoreText.text = points.ToString();
    }

    public void DrawCard()
    {
        StartCoroutine(APIClient.Instance.DrawCard(card =>
        {
            GameData.Instance.CurrentCard = card;

            if (card != null)
            {
                SceneManager.LoadScene("DrawnCardScene");
            }
            else
            {
                SceneManager.LoadScene("BossScene");
            }
            
        }));
    }
}