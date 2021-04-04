using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{

    public GameObject button;
    public GameObject drawCardObject;
    public GameObject mainMenuObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        drawCardObject.SetActive(true);
        mainMenuObject.SetActive(false);
    }

    public void help()
    {
        Debug.Log("Di do pýče");
    }
}
