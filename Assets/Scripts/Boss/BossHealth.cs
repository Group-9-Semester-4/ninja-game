using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public Text scoreText;

    public int score;

    // Start is called before the first frame update
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
