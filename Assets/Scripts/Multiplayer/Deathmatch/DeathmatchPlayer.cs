using API.Models;
using UnityEngine;
using UnityEngine.UI;

public class DeathmatchPlayer : MonoBehaviour
{
    public Text userName;
    public Player player;
    public GameObject statusColor;
    public Text scoreText;

    public bool ready;

    public void Ready()
    {
        ready = true;
        statusColor.SetActive(true);
        scoreText.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        statusColor.SetActive(false);
        scoreText.gameObject.SetActive(true);
    }

    public void SetUsername(string username)
    {
        userName.text = username;
    }
    
    public void SetScore(int score)
    {
        // Condition to prevent overwriting same score
        if (scoreText.text != score.ToString())
        {
            scoreText.text = score.ToString();
        }
    }
}
