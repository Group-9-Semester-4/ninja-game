using UnityEngine;
using UnityEngine.UI;
using Game;

public class BossScore : MonoBehaviour
{
    public Text scoreText;

    public int score;

    void Start()
    {
        scoreText.text = "Score: " + score;
    }

    public void TakeDamage(int damage = 1)
    {
        score += damage;

        if (score <= 0)
        {
            scoreText.text = "";
            return;
        }
        GameService.Instance.successfulHits++;
        scoreText.text = "Score: " + score;
    }

}
