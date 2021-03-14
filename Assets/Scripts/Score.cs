using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public float score;
    public Text ScoreText;


    void Start()
    {
        score = 0;
        ScoreText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(float points)
    {
        score = points + score;
        ScoreText.text = score.ToString();
    }

}