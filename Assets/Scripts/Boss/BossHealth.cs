using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public Text healthText;

    public int maxHealth = 100;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthText.text = "Health: " + health;
    }

    public void TakeDamage(int damage = 1)
    {
        health -= damage;

        if (health <= 0)
        {
            healthText.text = "";
            return;
        }
        
        healthText.text = "Health: " + health;
    }
}
