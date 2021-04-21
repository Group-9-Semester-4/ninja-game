using System.Collections;
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
        
        bossScript.bossScore.TakeDamage(); //change TakeDamage into what bodypart that is hit (use tags)
        bossScript.bossScore.TakeDamage(3); // Take damage to head
        bossScript.bossScore.TakeDamage(2); // Take damage to torso

        StartCoroutine(ColorChangeTime(bossScript));
    }

    IEnumerator ColorChangeTime(Boss bossScript)
    {
        // Change color to red
        bossScript.GetComponent<Renderer>().material.color = new Color(0.5f, 0f, 0f);
        
        // Wait one second
        yield return new WaitForSecondsRealtime(0.1F);

        // change color back to normal
        bossScript.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
        
        Destroy(gameObject);
    }
    
    private void Stick()
    {
        thisProjectile.constraints = RigidbodyConstraints.FreezeAll;
    }


}

