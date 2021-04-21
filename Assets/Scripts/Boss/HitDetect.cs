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


        var boss = col.gameObject;

        var bossScript = (Boss) boss.GetComponent(typeof(Boss));
        
        bossScript.bossScore.TakeDamage();
        if(Time.fixedDeltaTime < 0.5f) {
           
            bossScript.GetComponent<Renderer>().material.color = new Color(0.5f, 0f, 0f);
        }
        else
        {
            bossScript.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f);
        }
        
    }



    private void Stick()
    {
        thisProjectile.constraints = RigidbodyConstraints.FreezeAll;
    }


}

