using System.Collections;
using System.Collections.Generic;
using APIClient.Models;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        APIClient.APIClient.Instance.InitGame();
        
        // TODO: Add unwanted cards in separate step later
        var game = APIClient.APIClient.Instance.StartGame(new List<Card>());
        
        Debug.Log("Game created "+ game.uuid);
    }
}
