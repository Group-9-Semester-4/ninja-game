﻿using UnityEngine;

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
    }



    private void Stick()
    {
        thisProjectile.constraints = RigidbodyConstraints.FreezeAll;
    }
}

