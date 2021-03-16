using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float timeLeft = 3.0f;
    public Text startText;

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        startText.text = (timeLeft).ToString("0");
        if (timeLeft < 0)
        {
            //Debug.Log("Times up");
            
        }
    }
}
