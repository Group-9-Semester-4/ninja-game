using System;
using API;
using API.Models;
using API.Models.GameModes;
using Game;
using SocketIOClient;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicGame : MonoBehaviour
{
    public WebReq webReq;
    
    public GameObject playerPrefab;
    public Transform playerContainer;

    public GameObject drawCardButton;
    public GameObject completeButton;

    public GameObject loadingImage;
    public GameObject cardImage;
    
    private SocketIO _socketIO;

    private bool _isOnTurn;

    private bool _refresh;

    void Start()
    {
        _socketIO = SocketClient.Client;
        
        _socketIO.On("game-update", response =>
        {
            GameData.Instance.GameInfo = response.GetValue<GameInfo>();
            _refresh = true;
        });
        
        RefreshPlayerData();
    }

    private void Update()
    {
        if (_refresh)
        {
            _refresh = false;
            RefreshPlayerData();
        }
    }

    public void DrawCard()
    {
        drawCardButton.SetActive(false);
        _socketIO.EmitAsync("basic.draw", new object());
    }
    
    public void Complete()
    {
        completeButton.SetActive(false);
        _socketIO.EmitAsync("basic.complete", new object());
    }

    public void RefreshPlayerData()
    {
        CleanPlayerContainer();

        _isOnTurn = false;
        
        drawCardButton.SetActive(_isOnTurn);

        var gameInfo = GameData.Instance.GameInfo;

        var gameModeData = (BasicGameMode) gameInfo.gameModeData.ToObject(typeof(BasicGameMode));

        if (gameModeData.drawnCard == null)
        {
            if (gameModeData.remainingCards.Count == 0)
            {
                // Move on to boss fight
                StartBossFight();
                return;
            }
            InstantiatePlayers(gameModeData);
            webReq.HideCard();
            SetImagesActive(false);
            drawCardButton.SetActive(_isOnTurn);
        }
        else
        {
            InstantiateDrawnCardPlayers(gameModeData);
            completeButton.SetActive(true);
            SetImagesActive(true);
            webReq.card = gameModeData.drawnCard;
            webReq.RenderCard();
        }
    }


    // Helper methods

    private void StartBossFight()
    {
        GameData.Instance.IsMultiplayer = true;
        
        var gameInfo = GameData.Instance.GameInfo;

        var gameModeData = (BasicGameMode) gameInfo.gameModeData.ToObject(typeof(BasicGameMode));

        GameData.Instance.Points = (int) (Math.Sqrt(gameModeData.score) * 1.5);
        
        SceneManager.LoadScene("Scenes/BossScene");
    }

    private void InstantiateDrawnCardPlayers(BasicGameMode gameModeData)
    {
        foreach (var player in gameModeData.players)
        {
            var playerGameObject = Instantiate(playerPrefab, playerContainer);

            var playerScript = playerGameObject.GetComponent<PlayerScript>();

            playerScript.player = player;

            var cardCompleted = false;

            if (gameModeData.completeStates.ContainsKey(player.sessionId))
            {
                cardCompleted = gameModeData.completeStates[player.sessionId];
            }
                
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
                
                if (player.sessionId == _socketIO.Id)
                {
                    _isOnTurn = true;
                }
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

    private void SetImagesActive(bool state)
    {
        loadingImage.SetActive(state);
        cardImage.SetActive(state);
    }
}
