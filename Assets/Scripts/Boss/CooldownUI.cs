using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownUI : MonoBehaviour
{
    public GameObject cooldownBar;
    public Vector3 scaleChange;

    // Start is called before the first frame update
    void Start()
    {
        scaleChange = new Vector3(0.1f, 0f, 0f);
    }

    private void FixedUpdate()
    {
        if (cooldownBar.transform.localScale.x < 4f)
        {
            cooldownBar.transform.localScale += scaleChange;
        }
    }
}
