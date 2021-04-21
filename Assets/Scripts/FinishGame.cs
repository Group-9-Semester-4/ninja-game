using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;

public class FinishGame : MonoBehaviour
{
    
    public Text textScore;
    public Text cardsDone;
    public Text accuracy;

    // Start is called before the first frame update
    void Start()
    {
        textScore.text = "Final score: "+GameService.Instance.score;
        cardsDone.text = "Cards completed: "+GameService.Instance.cardsCompleted;
        Debug.Log(GameService.Instance.successfulHits);
        Debug.Log(GameService.Instance.ammo);
        double accuracyInPercent = ((double)GameService.Instance.successfulHits/GameService.Instance.ammo)*100;
        accuracy.text = "Your throwing accuracy was: "+(int)accuracyInPercent+"%";

        GameService.Instance.score = 0;
        GameService.Instance.cardsCompleted = 0;
        GameService.Instance.successfulHits = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
