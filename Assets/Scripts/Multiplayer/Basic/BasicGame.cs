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

public class BasicGame : MonoBehaviour
{
    public WebReq webReq;
    
    public GameObject timer;
    public Text timerText;
    public float timeLeft;
    public bool timerStarted;
    public bool timerFinished;
    public bool timerStopped;
    
    public GameObject stopTimerButton;
    public GameObject startTimerButton;
    
    public GameObject playerPrefab;
    public Transform playerContainer;

    public GameObject drawCardButton;
    public GameObject completeButton;

    public GameObject loadingImage;
    public GameObject cardImage;

    public GameObject cardInfo;
    public Text cardDescription;
    public Text cardRepetitions;
    
    public Card currentCard;
    private SocketIOController _socketIO;

    private bool _isOnTurn;
    private bool _refresh;

    void Start()
    {
        cardInfo.SetActive(false);
        stopTimerButton.SetActive(false);
        _socketIO = SocketClient.Client;
        
        _socketIO.On("game-update", response =>
        {
            GameData.Instance.GameInfo = Helper.DeserializeGameInfo(response.data);
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
                stopTimerButton.SetActive(true);
            }
        }

        if (timerFinished)
        {
            // Timer was finished start blinking time left
            timerText.text = Time.fixedTime % .5 < .2 ? "" : "0";
        }
    }

    public void DrawCard()
    {
        drawCardButton.SetActive(false);
        _socketIO.Emit("basic.draw");
    }
    
    public void Complete()
    {
        completeButton.SetActive(false);
        _socketIO.Emit("basic.complete");
    }

    public void RefreshPlayerData()
    {
        CleanPlayerContainer();

        _isOnTurn = false;
        
        drawCardButton.SetActive(_isOnTurn);

        var gameInfo = (BasicGameModeGameInfo) GameData.Instance.GameInfo;

        var gameModeData = gameInfo.GameModeData();

        HideTimer();

        if (gameModeData.drawnCard?.id == null)
        {
            if (gameModeData.remainingCards.Count == 0)
            {
                // Move on to boss fight
                StartBossFight();
                return;
            }
            InstantiatePlayers(gameModeData);
            webReq.HideCard();
            cardInfo.SetActive(false);
            SetImagesActive(false);
            drawCardButton.SetActive(_isOnTurn);
        }
        else
        {
            SetCardInfo(gameModeData.drawnCard);
            cardInfo.SetActive(true);
            InstantiateDrawnCardPlayers(gameModeData);
            
            if (gameModeData.HasCompleted(_socketIO.SocketID))
            {
                completeButton.SetActive(false);
            }
            else
            {
                completeButton.SetActive(true);
            }
            
            SetImagesActive(true);
            webReq.card = gameModeData.drawnCard;
            webReq.RenderCard();
            if (gameModeData.drawnCard.hasTimer)
            {
                ShowTimer(gameModeData.drawnCard.difficulty);
            }
        }
    }


    // Helper methods

    private void StartBossFight()
    {
        GameData.Instance.IsMultiplayer = true;
        
        var gameInfo = (BasicGameModeGameInfo) GameData.Instance.GameInfo;

        var gameModeData = gameInfo.GameModeData();

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

            var cardCompleted = gameModeData.HasCompleted(player.sessionId);

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
                
                if (player.sessionId == _socketIO.SocketID)
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
}
