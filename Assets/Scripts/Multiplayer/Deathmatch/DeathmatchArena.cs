using System.Collections;
using System.Collections.Generic;
using API;
using API.Models;
using API.Models.GameModes;
using API.Models.HelperModels;
using API.Params;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    public Image cardImage;

    public GameObject readyButton;

    private SocketIOController _socketIO;
    private Card _drawnCard;

    public bool started;

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
            GameUpdate();
        });
        
        _socketIO.On("game-finish", response =>
        {
            GameData.Instance.GameInfo = Helper.DeserializeGameInfo(response.data);
            SceneManager.LoadScene("AfterDeathmatch");
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

    public void OnReady()
    {
        _socketIO.Emit("deathmatch.ready");
        readyButton.SetActive(false);
    }

    public void OnCardClick(Card card)
    {
        if (!started)
        {
            return;
        }
        
        var param = new LockCardParam
        {
            cardId = card.id, 
            playerId = _socketIO.SocketID
        };
        
        _socketIO.Emit("deathmatch.lock-card", JsonUtility.ToJson(param), result =>
        {
            var message = JsonUtility.FromJson<SocketIOMessage>(result);
            if (message.IsSuccess())
            {
                _drawnCard = card;
                OpenCardModal();
            }
        });
    }

    public void OpenCardModal()
    {
        cardPanel.SetActive(true);

        cardDescription.text = _drawnCard.description;
        cardRepetitions.text = _drawnCard.difficulty.ToString();

        webReq.card = _drawnCard;
        webReq.image = cardImage;
        webReq.RenderCard();
    }

    public void CloseCardModal()
    {
        var param = new LockCardParam
        {
            cardId = _drawnCard.id, 
            playerId = _socketIO.SocketID
        };
        
        _socketIO.Emit("deathmatch.unlock-card", JsonUtility.ToJson(param), result =>
        {
            var message = JsonUtility.FromJson<SocketIOMessage>(result);
            if (message.IsSuccess())
            {
                cardPanel.SetActive(false);
                cardImage.gameObject.SetActive(false);
            }
        });;
    }

    public void CardComplete()
    {
        var param = new CardCompleteParam
        {
            cardId = _drawnCard.id, 
            playerId = _socketIO.SocketID
        };
        
        _socketIO.Emit("deathmatch.complete", JsonUtility.ToJson(param), result =>
        {
            cardPanel.SetActive(false);
            cardImage.gameObject.SetActive(false);
        });;
    }

    public void GameUpdate()
    {
        var gameInfo = (DeathmatchGameModeGameInfo) GameData.Instance.GameInfo;

        foreach (Transform playerTransform in playerContainer)
        {
            var playerGameObject = playerTransform.gameObject;
            
            var playerScript = playerGameObject.GetComponent<DeathmatchPlayer>();

            var player = playerScript.player;

            var playerScore = gameInfo.gameModeData.playerScores.Find(score => score.playerId == player.sessionId);

            if (playerScore != null)
            {
                playerScript.SetScore(playerScore.score);
            }
            
        }

        foreach (Transform cardTransform in cardContainer)
        {
            var cardGameObject = cardTransform.gameObject;
            
            var cardScript = cardGameObject.GetComponent<DeathmatchCard>();

            var card = cardScript.card;

            var cardLockInfo = gameInfo.gameModeData.remainingCards.Find(lockInfo => lockInfo.card.id == card.id);

            if (cardLockInfo != null)
            {
                cardScript.SetLocked(cardLockInfo.locked);
            }
            else
            {
                Destroy(cardGameObject);
            }
        }
        
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

            card.AddComponent(typeof(EventTrigger));
            
            var trigger = card.GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) =>
            {
                OnCardClick(cardLockInfo.card);
            });
            
            trigger.triggers.Add(entry);
            
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
        started = true;
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
