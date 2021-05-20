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
        setPositionList.Add(new Vector3(-89f, -4.916667f, 8.8f));
        setPositionList.Add(new Vector3(-18f, -4.916667f, 46.4f));
        setPositionList.Add(new Vector3(-14.8f, -4.916667f, 26.4f));
        setPositionList.Add(new Vector3(26.2f, -4.916667f, -29.9f));
        setPositionList.Add(new Vector3(80.7f, -4.916667f, 49.1f));
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
