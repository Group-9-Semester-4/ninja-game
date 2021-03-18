using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public float score;
    public Text ScoreText;

    // Update is called once per frame
    void Update()
    {
        var points = GameData.Instance.Points;

        ScoreText.text = points.ToString();
    }


}