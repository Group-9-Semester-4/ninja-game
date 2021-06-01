using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossNavMovement : MonoBehaviour
{
    // AI component for the boss
    public NavMeshAgent agent;

    //actual position of the boss
    public Vector3 currentPosition;

    //postion the boss is moving towards
    public Vector3 setPosition;



    // Need an array/list to store position values the boss can move to
    public List<Vector3> setPositionList = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        setPositionList.Add(new Vector3(18.7f, -4.966667f, 37.7f));
        setPositionList.Add(new Vector3(-13.5f, -4.966667f, 23.5f));
        setPositionList.Add(new Vector3(-26.7f, -4.966667f, 51.9f));
        setPositionList.Add(new Vector3(-46.1f, -4.966667f, 51.6f));
       
        GetNewPosition();

    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;


        if(currentPosition == setPosition)
        {
            GetNewPosition();
        }

        agent.SetDestination(setPosition);

    }


    public void GetNewPosition()
    {
        setPosition = setPositionList[Random.Range(0, setPositionList.Count)];
    }
}
