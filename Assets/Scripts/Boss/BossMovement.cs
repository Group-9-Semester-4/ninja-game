using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    //move speed (how much the translation value should change per update)
    public float speed = 0.05f;

    //translation values ( current movement values)
    public float x;
    public float y;
    public float z;


    //world coordinate values for position
    public float xPos;
    public float yPos;
    public float zPos;
   
    //direction of boss movement
    bool movingLeft = false;
    bool movingRight = true;

    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        zPos = transform.position.z;
        if (movingLeft == true)
        {
            transform.Translate(x + speed, y, z);
            if (xPos < -9.5)
            {
                movingLeft = false;
                movingRight = true;
            }
        }
        if (movingRight == true)
        {
            transform.Translate(x - speed, y, z);
            if (xPos > 9.5)
            {
                movingRight = false;
                movingLeft = true;
            }

        }
    }
}
