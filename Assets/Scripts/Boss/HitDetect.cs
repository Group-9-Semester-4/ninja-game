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
        if (hit)
        {
            Destroy(gameObject);
            return;
        }
        
        hit = true;

        var boss = col.gameObject;

        var bossScript = (Boss) boss.GetComponent(typeof(Boss));


        switch (col.gameObject.tag)
        {
            case "Head":
            {
                bossScript.bossScore.TakeDamage(3);
                bossScript.TakeHit();
                break;
            }
            case "Torso":
            {
                bossScript.bossScore.TakeDamage(2);
                bossScript.TakeHit();
                break;
            }
            case "ArmLeg":
            {
                bossScript.bossScore.TakeDamage();
                bossScript.TakeHit();
                break;
            }
        }

        Destroy(gameObject);
    }

    private void Stick()
    {
        thisProjectile.constraints = RigidbodyConstraints.FreezeAll;
    }


}

