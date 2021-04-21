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

    public void TakeDamageArmLeg(int damage = 1)
    {
        score += damage;

        if (score <= 0)
        {
            scoreText.text = "";
            return;
        }
        
        scoreText.text = "Score: " + score;
    }

    public void TakeDamageHead(int damage = 3)
    {
        score += damage;

        if (score <= 0)
        {
            scoreText.text = "";
            return;
        }

        scoreText.text = "Score: " + score;
    }

    public void TakeDamageTorso(int damage = 2)
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
