using API.Models.GameModes;
using Game;
using UnityEngine;

public class BasicGame : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerContainer;
    
    void Start()
    {
        RefreshPlayerData();
    }

    void Update()
    {
        
    }

    public void RefreshPlayerData()
    {
        CleanPlayerContainer();
        
        var gameInfo = GameData.Instance.GameInfo;

        var gameModeData = (BasicGameMode) gameInfo.gameModeData.ToObject(typeof(BasicGameMode));

        if (gameModeData.drawnCard == null)
        {
            foreach (var player in gameModeData.players)
            {
                var playerGameObject = Instantiate(playerPrefab, playerContainer);

                var playerScript = playerGameObject.GetComponent<PlayerScript>();

                playerScript.player = player;

                if (gameModeData.playerOnTurn == player.sessionId)
                {
                    playerScript.setOnTurn();
                }
                else
                {
                    playerScript.setPassive();
                }

            }
        }
        else
        {
            foreach (var player in gameModeData.players)
            {
                var playerGameObject = Instantiate(playerPrefab, playerContainer);

                var playerScript = playerGameObject.GetComponent<PlayerScript>();

                playerScript.player = player;

                var cardCompleted = gameModeData.completeStates[player.sessionId];
                
                if (cardCompleted)
                {
                    playerScript.setComplete();
                }
                else
                {
                    playerScript.setPassive();
                }

            }
        }
        
    }

    private void CleanPlayerContainer()
    {
        var childCount = playerContainer.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
        {
            Destroy(playerContainer.GetChild(i).gameObject);
        }
    }
}
