using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDelete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    //attach this script to any object you dont want to be destroyed/deleted when the scenes are being changed.
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
