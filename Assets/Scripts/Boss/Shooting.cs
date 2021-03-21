using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public float bulletZ = 8f;
    public float force = 50f;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Vector2 screenMousePos = Input.mousePosition;
        UnityEngine.Vector2 mousePos = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(screenMousePos.x, screenMousePos.y, bulletZ));

        if(Input.GetMouseButtonDown(0))
        {
            GameObject firedBullet = Instantiate(bullet, transform.position, UnityEngine.Quaternion.identity);

            UnityEngine.Vector3 bulletDirection = new UnityEngine.Vector3(mousePos.x, mousePos.y, bulletZ) - transform.position;

            firedBullet.GetComponent<Rigidbody>().velocity += force * bulletDirection;
        }
    }
}
