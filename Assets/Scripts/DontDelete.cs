using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDelete : MonoBehaviour
{

    //attach this script to any object you dont want to be destroyed/deleted when the scenes are being changed.
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}