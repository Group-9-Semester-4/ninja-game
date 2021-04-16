using UnityEngine;

public class HitDetect : MonoBehaviour
{
    public Rigidbody thisProjectile;
    private bool hit = false;

    private void OnCollisionEnter(Collision collision)
    {
        hit = true;
        Stick();
    }

    void OnTriggerEnter(Collider col)
    {

        //Destroy(gameObject);

        var boss = col.gameObject;

        var bossScript = (Boss) boss.GetComponent(typeof(Boss));
        
        bossScript.bossHealth.TakeDamage();

        if (bossScript.bossHealth.health <= 0)
        {
            Destroy(boss);
            bossScript.FinishGame();
        }
    }



    private void Stick()
    {
        thisProjectile.constraints = RigidbodyConstraints.FreezeAll;
    }
}

