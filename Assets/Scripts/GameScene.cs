using System.Collections;
using System.Collections.Generic;
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
        var card = APIClient.APIClient.Instance.DrawCard();
        GameData.Instance.CurrentCard = card;

        SceneManager.LoadScene("DrawnCardScene");
    }
}