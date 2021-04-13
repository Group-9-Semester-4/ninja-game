using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public static int HitCount;
    
    public GameObject AmmoClass;
    public Text hitCount;

    public float bulletZ = 8f;
    public float force = 50f;
    public GameObject bullet;

  
    float lastShot;
    public float delay = 1;


    public GameObject cooldownBar;
    Ammo ammosource;
    // Start is called before the first frame update
    void Start()
    {
        lastShot = Time.time;

        HitCount = 0;

        ammosource = AmmoClass.GetComponent<Ammo>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - delay > lastShot))
        {
            UnityEngine.Vector2 screenMousePos = Input.mousePosition;
            UnityEngine.Vector2 mousePos = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(screenMousePos.x, screenMousePos.y, bulletZ));

            if (Input.GetMouseButtonDown(0) && ammosource.ammo > 0)
            {
                GameObject firedBullet = Instantiate(bullet, transform.position, UnityEngine.Quaternion.identity);

                UnityEngine.Vector3 bulletDirection = new UnityEngine.Vector3(mousePos.x, mousePos.y, bulletZ) - transform.position;

                firedBullet.GetComponent<Rigidbody>().velocity += force * bulletDirection;
                lastShot = Time.time;

                cooldownBar.transform.localScale = new UnityEngine.Vector3(0f, 0.3f, 1f);

                ammosource.ammo--;
            }
        }
        
        hitCount.text = "Hits: " + HitCount;
    }
}
