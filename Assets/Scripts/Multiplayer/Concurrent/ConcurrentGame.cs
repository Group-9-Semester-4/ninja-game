using System;
using API;
using API.Models;
using API.Models.GameModes;
using Game;
using SocketIOClient;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConcurrentGame : MonoBehaviour
{
    public WebReq webReq;
    
    public GameObject playerPrefab;
    public Transform playerContainer;

    public GameObject drawCardButton;
    public GameObject completeButton;

    public GameObject loadingImage;
    public GameObject cardImage;
    
    private SocketIO _socketIO;
    
    private bool _refresh;

    private Card _drawnCard;
    private bool _refreshCard;

    private bool _cardComplete;

    private bool _startBoss;

    // Start is called before the first frame update
    void Start()
    {
        _socketIO = SocketClient.Client;
        
        _socketIO.On("game-update", response =>
        {
            GameData.Instance.GameInfo = response.GetValue<GameInfo>();
            _refresh = true;
        });
        
        _socketIO.On("boss-start", response =>
        {
            GameData.Instance.GameInfo = response.GetValue<GameInfo>();
            _startBoss = true;
        });
        
        RefreshPlayerData();
        DrawCard();
    }

    // Update is called once per frame
    void Update()
    {
        if (_refresh)
        {
            _refresh = false;
            RefreshPlayerData();
        }

        if (_refreshCard)
        {
            _refreshCard = false;
            webReq.card = _drawnCard;
            webReq.HideCard();
            webReq.RenderCard();
        }

        if (_cardComplete)
        {
            _cardComplete = false;
            SetButtonsActive(true);
            DrawCard();
        }

        if (_startBoss)
        {
            StartBossFight();
        }
    }

    public void DrawCard()
    {
        _socketIO.EmitAsync("concurrent.draw", response =>
        {
            var message = response.GetValue<DrawCardMessage>();

            if (message.IsSuccess())
            {
                _drawnCard = message.data;
                _refreshCard = true;
            }
        });
    }

    public void Complete()
    {
        SetButtonsActive(false);
        webReq.HideCard();
        _socketIO.EmitAsync("concurrent.complete", response =>
        {
            _cardComplete = true;
        });
    }

    public void RefreshPlayerData()
    {
        CleanPlayerContainer();

        var gameInfo = GameData.Instance.GameInfo;

        var gameModeData = (ConcurrentGameMode) gameInfo.gameModeData.ToObject(typeof(ConcurrentGameMode));
        
        InstantiatePlayers(gameModeData);
    }

    public void StartBossFight()
    {
        var instance = GameData.Instance;
        
        instance.IsMultiplayer = true;
            
        var gameModeData = (ConcurrentGameMode) instance.GameInfo.gameModeData.ToObject(typeof(ConcurrentGameMode));

        var score = gameModeData.playerScores[_socketIO.Id];
            
        instance.Points = (int) (Math.Sqrt(score) * 1.5);
        
        SceneManager.LoadScene("Scenes/BossScene");
    }
    
    private void InstantiatePlayers(ConcurrentGameMode gameModeData)
    {
        foreach (var player in gameModeData.players)
        {
            var cardsDone = 0;

            if (gameModeData.numberOfPlayerCardsDone.ContainsKey(player.sessionId))
            {
                cardsDone = gameModeData.numberOfPlayerCardsDone[player.sessionId];
            }
            
            var playerGameObject = Instantiate(playerPrefab, playerContainer);

            var playerScript = playerGameObject.GetComponent<ConcurrentPlayerScript>();

            playerScript.SetUserName(player.name);
            playerScript.SetDoneAmount(cardsDone);

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

    private void SetButtonsActive(bool active)
    {
        drawCardButton.SetActive(active);
        completeButton.SetActive(active);
    }
    
}
