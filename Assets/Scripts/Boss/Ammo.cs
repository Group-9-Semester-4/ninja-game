using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    public int ammo;
    public Text ammoTxt;
    // Start is called before the first frame update
    void Start()
    {
        var points = GameData.Instance.Points;

        if (points != 0)
        {
            ammo = points;
        }
        else
        {
            ammo = 5000;
        }
        ammo = 5000;
    }


    // Update is called once per frame
    void Update()
    {
        ammoTxt.text = "Ammo: " + ammo;
    }
}
