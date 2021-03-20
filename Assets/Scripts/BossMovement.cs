using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float speed = 0.05f;

    //translation values ( current movement speed values)
    public float x;
    public float y;
    public float z;


    //world coordinate values
    public float xPos;
    public float yPos;
    public float zPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(x + speed, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(transform.position.x > 9.5)
        {

        }
        */
    }
}
