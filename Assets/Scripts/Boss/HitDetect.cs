using UnityEngine;

public class HitDetect : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {

        Destroy(gameObject);

        var boss = col.gameObject;

        var bossScript = (Boss) boss.GetComponent(typeof(Boss));
        
        bossScript.bossHealth.TakeDamage();

        if (bossScript.bossHealth.health <= 0)
        {
            Destroy(boss);
            bossScript.FinishGame();
        }
    }
}
