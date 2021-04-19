using UnityEngine;
using UnityEngine.UI;

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
        
        scoreText.text = "Score: " + score;
    }
}
