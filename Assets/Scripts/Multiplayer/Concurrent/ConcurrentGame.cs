using System;
using API;
using API.Models;
using API.Models.GameModes;
using Game;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnitySocketIO;
using UnitySocketIO.SocketIO;

public class ConcurrentGame : MonoBehaviour
{
    public WebReq webReq;

    public GameObject timer;
    public GameObject cardInfo;
    public Text timerText;
    public float timeLeft;
    public bool timerStarted;
    public bool timerFinished;
    public bool timerStopped;

    public GameObject stopTimerButton;
    public GameObject startTimerButton;
    
    public GameObject playerPrefab;
    public Transform playerContainer;

    public Text cardDescription;
    public Text cardRepetitions;
    
    public GameObject drawCardButton;
    public GameObject completeButton;

    public GameObject loadingImage;
    public GameObject cardImage;

    private SocketIOController _socketIO;
    private Card _drawnCard;

    private bool _refresh;
    private bool _refreshCard;
    private bool _cardComplete;
    private bool _startBoss;

    // Start is called before the first frame update
    void Start()
    {
        cardInfo.SetActive(false);
        stopTimerButton.SetActive(false);
        _socketIO = SocketClient.Client;

        _socketIO.On("game-update", response =>
        {
            GameData.Instance.GameInfo = JsonConvert.DeserializeObject<GameInfo>(response.data);
            _refresh = true;
        });

        _socketIO.On("boss-start", response =>
        {
            GameData.Instance.GameInfo = JsonConvert.DeserializeObject<GameInfo>(response.data);
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

        if (!timerFinished && timerStarted && !timerStopped)
        {
            startTimerButton.SetActive(false);
            stopTimerButton.SetActive(true);
            timeLeft -= Time.deltaTime;
            timerText.text = (timeLeft).ToString("0");
            if (timeLeft < 0)
            {
                stopTimerButton.SetActive(false);
                completeButton.SetActive(true);
                timer.SetActive(false);
                timerFinished = true;
            }
            else
            {
                cardInfo.SetActive(true);
                stopTimerButton.SetActive(true);
            }
        }

        if (timerFinished)
        {
            // Timer was finished start blinking time left
            timerText.text = Time.fixedTime % .5 < .2 ? "" : "0";
        }

        if (_refreshCard)
        {
            _refreshCard = false;
            webReq.card = _drawnCard;
            webReq.HideCard();
            webReq.RenderCard();
            if (_drawnCard.hasTimer)
            {
                ShowTimer(_drawnCard.difficulty);
            }
            else {
                SetButtonsActive(true);
            }
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
        HideTimer();
        _socketIO.Emit("concurrent.draw", response =>
        {
            var message = JsonUtility.FromJson<DrawCardMessage>(response);

            if (message.IsSuccess())
            {
                _drawnCard = message.data;
                SetCardInfo(_drawnCard);
                _refreshCard = true;
            }
        });
    }

    public void Complete()
    {
        SetButtonsActive(false);
        webReq.HideCard();
        _socketIO.Emit("concurrent.complete", response =>
        {
            _cardComplete = true;
        });
    }

    public void RefreshPlayerData()
    {
        CleanPlayerContainer();

        var gameInfo = GameData.Instance.GameInfo;

        var gameModeData = (ConcurrentGameMode)gameInfo.gameModeData.ToObject(typeof(ConcurrentGameMode));

        InstantiatePlayers(gameModeData);
    }

    public void StartBossFight()
    {
        var instance = GameData.Instance;

        instance.IsMultiplayer = true;

        var gameModeData = (ConcurrentGameMode)instance.GameInfo.gameModeData.ToObject(typeof(ConcurrentGameMode));

        var score = gameModeData.playerScores[_socketIO.SocketID];

        instance.Points = (int)(Math.Sqrt(score) * 1.5);

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

    public void StartTimer()
    {
        timerStarted = true;
        timerStopped = false;
    }
    
    public void StopTimer()
    {
        timerStopped = true;
        startTimerButton.SetActive(true);
        stopTimerButton.SetActive(false);
        completeButton.SetActive(false);
    }

    private void ShowTimer(int seconds)
    {
        completeButton.SetActive(false);
        timerStarted = false;
        timerFinished = false;

        timeLeft = seconds;
        timer.SetActive(true);
        timerText.text = (timeLeft).ToString("0");
    }

    private void HideTimer()
    {
        timer.SetActive(false);
    }
    
    private void SetCardInfo(Card card)
    {
        cardDescription.text = card.description;
    
        if (card.hasTimer)
        {
            cardRepetitions.text = card.difficulty + " seconds";
        }
        else
        {
            cardRepetitions.text = card.difficulty + " repetitions";
        }
    }

}
