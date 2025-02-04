﻿using UnityEngine;

public class Shooting : MonoBehaviour
{
    
    public GameObject AmmoClass;

    public float bulletZ = 8f;
    public float force = 50f;
    public GameObject bullet;

    Ammo ammosource;
    
    // Start is called before the first frame update
    void Start()
    {

        ammosource = AmmoClass.GetComponent<Ammo>();

    }

    // Update is called once per frame
    void Update()
    {
        
        UnityEngine.Vector2 screenMousePos = Input.mousePosition;
        UnityEngine.Vector2 mousePos = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(screenMousePos.x, screenMousePos.y, bulletZ));

        if (Input.GetMouseButtonDown(0) && ammosource.ammo > 0)
        {
            GameObject firedBullet = Instantiate(bullet, transform.position, UnityEngine.Quaternion.identity);

            UnityEngine.Vector3 bulletDirection = new UnityEngine.Vector3(mousePos.x, mousePos.y, bulletZ);

            firedBullet.GetComponent<Rigidbody>().velocity += force * bulletDirection;

            ammosource.ammo--;
        }
    }
}
