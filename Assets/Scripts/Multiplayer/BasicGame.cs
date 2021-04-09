using API;
using API.Models.GameModes;
using Game;
using SocketIOClient;
using UnityEngine;

public class BasicGame : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerContainer;

    private SocketIO _socketIO;
    
    void Start()
    {
        _socketIO = SocketClient.Client;
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
            InstantiatePlayers(gameModeData);
        }
        else
        {
            InstantiateDrawnCardPlayers(gameModeData);
        }
        
    }
    
    
    
    
    
    // Helper methods

    private void InstantiateDrawnCardPlayers(BasicGameMode gameModeData)
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
    
    private void InstantiatePlayers(BasicGameMode gameModeData)
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

    private void CleanPlayerContainer()
    {
        var childCount = playerContainer.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
        {
            Destroy(playerContainer.GetChild(i).gameObject);
        }
    }
}
