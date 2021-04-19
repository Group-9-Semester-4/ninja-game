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
    }



    private void Stick()
    {
        thisProjectile.constraints = RigidbodyConstraints.FreezeAll;
    }
}

