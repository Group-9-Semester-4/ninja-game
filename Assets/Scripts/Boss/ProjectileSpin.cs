using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpin : MonoBehaviour
{
    public float Speed;
    public float AngularSpeed;
    protected Rigidbody r;


    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();


        // Normally unity has set the max angular velocity to 7 which is not alot
        r.maxAngularVelocity = 100f;

        // The reason we use forcemode impulse is that "AddTorque" is a physics force that normally need to build up.
        r.AddTorque(Vector3.down, ForceMode.Impulse);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //magnitude returns the length of the vector
        Speed = r.velocity.magnitude;

        //angularvelocity is in radians per sec
        AngularSpeed = r.angularVelocity.magnitude;


    }
}
