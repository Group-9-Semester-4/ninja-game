using Game;
using UnityEngine;
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
        StartCoroutine(APIClient.APIClient.Instance.DrawCard());
    }
}