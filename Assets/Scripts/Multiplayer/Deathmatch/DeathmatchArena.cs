using System.Collections;
using System.Collections.Generic;
using API;
using API.Models;
using API.Models.GameModes;
using API.Models.HelperModels;
using Game;
using UnityEngine;
using UnityEngine.UI;
using UnitySocketIO;

public class DeathmatchArena : MonoBehaviour
{
    public WebReq webReq;

    public GameObject playerPrefab;
    public Transform playerContainer;

    public GameObject cardPrefab;
    public Transform cardContainer;

    public Text cardDescription;
    public Text cardRepetitions;

    public GameObject cardPanel;

    public GameObject loadingImage;
    public GameObject cardImage;

    public GameObject readyButton;

    private SocketIOController _socketIO;
    private Card _drawnCard;

    void Start()
    {
        _socketIO = SocketClient.Client;

        _socketIO.On("ready-update", response =>
        {
            GameData.Instance.GameInfo = Helper.DeserializeGameInfo(response.data);
            
            var gameInfo = (DeathmatchGameModeGameInfo) GameData.Instance.GameInfo;
            
            UpdatePlayerStates(gameInfo.gameModeData.playersReady);
        });

        _socketIO.On("game-update", response =>
        {
            GameData.Instance.GameInfo = Helper.DeserializeGameInfo(response.data);
        });
        
        _socketIO.On("start", response =>
        {
            GameData.Instance.GameInfo = Helper.DeserializeGameInfo(response.data);
            StartGame();
        });

        
        var gameInfo = (DeathmatchGameModeGameInfo) GameData.Instance.GameInfo;
        
        InitPlayerData(gameInfo);
        InitCardData(gameInfo);
    }

    public void onReady()
    {
        _socketIO.Emit("deathmatch.ready");
        readyButton.SetActive(false);
    }

    public void InitPlayerData(DeathmatchGameModeGameInfo gameInfo)
    {
        CleanPlayerContainer();

        var gameModeData = gameInfo.GameModeData();

        InstantiatePlayers(gameModeData);
    }

    public void InitCardData(DeathmatchGameModeGameInfo gameInfo)
    {
        foreach (var cardLockInfo in gameInfo.GameModeData().remainingCards)
        {
            var card = Instantiate(cardPrefab, cardContainer);

            var cardScript = card.GetComponent<DeathmatchCard>();

            cardScript.Init(cardLockInfo.card);

            if (cardLockInfo.locked)
            {
                cardScript.Lock();
            }
        }
    }

    public void UpdatePlayerStates(List<string> playersReady)
    {
        foreach (Transform playerTransform in playerContainer)
        {
            var playerGameObject = playerTransform.gameObject;
            
            var playerScript = playerGameObject.GetComponent<DeathmatchPlayer>();

            if (!playerScript.ready && playersReady.Contains(playerScript.player.sessionId))
            {
                playerScript.Ready();
            }
        }
    }

    public void StartGame()
    {
        foreach (Transform playerTransform in playerContainer)
        {
            var playerGameObject = playerTransform.gameObject;
            
            var playerScript = playerGameObject.GetComponent<DeathmatchPlayer>();

            playerScript.GameStart();
        }
    }
    
    private void InstantiatePlayers(DeathmatchGameMode gameModeData)
    {
        foreach (var player in gameModeData.players)
        {
            var playerGameObject = Instantiate(playerPrefab, playerContainer);

            var playerScript = playerGameObject.GetComponent<DeathmatchPlayer>();

            playerScript.player = player;
            playerScript.SetUsername(player.name);
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
