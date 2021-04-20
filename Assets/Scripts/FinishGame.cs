using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;

public class FinishGame : MonoBehaviour
{
    
    public Text textScore;

    // Start is called before the first frame update
    void Start()
    {
        textScore.text = "Final score: "+GameService.Instance.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
