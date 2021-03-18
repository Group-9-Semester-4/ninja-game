using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class DrawnCardScene : MonoBehaviour
{
    public void CompleteCard()
    {
        var gameData = GameData.Instance;

        gameData.Points += gameData.CurrentCard.points;
        
        gameData.CurrentCard = null;
    }
}
