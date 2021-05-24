using System;
using System.Collections.Generic;
using API;
using API.Models;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnitySocketIO;

public class Lobby : DiscardCardsScript
{
    public GameObject playerPrefab;
    public Transform playerContainer;
    public Dropdown gameModesDropdown;

    public bool reloadLobby;
    public bool startGame;

    public GameObject[] lobbyLeaderObjects;

    private SocketIOController socketIOController;
    private List<string> _gameModes;
    
    void Start()
    {
        base.Start();

        socketIOController = SocketClient.Client;
        
        socketIOController.On("lobby-update", response =>
        {
            GameData.Instance.GameInfo = Helper.DeserializeGameInfo(response.data);

            reloadLobby = true;
        });
        
        socketIOController.On("start", response =>
        {
            GameData.Instance.GameInfo = Helper.DeserializeGameInfo(response.data);
            GameData.Instance.IsMultiplayer = true;

            startGame = true;
        });

        var routine = APIClient.Instance.GetGameModes(gameModes =>
        {
            _gameModes = new List<string>() { "undefined" };
            foreach (var gameMode in gameModes)
            {
                var optionItem = new Dropdown.OptionData(gameMode);
                gameModesDropdown.options.Add(optionItem);
                _gameModes.Add(gameMode);
            }
        });

        StartCoroutine(routine);

        InitLobbyData();
    }

    private void Update()
    {
        if (reloadLobby)
        {
            InitLobbyData();
            reloadLobby = false;
        }

        if (startGame)
        {
            var gameMode = GameData.Instance.GameInfo.GameModeId();

            switch (gameMode)
            {
                case "basic":
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/Basic/BasicGame");
                    break;
                }
                case "concurrent":
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/Concurrent/Concurrent");
                    break;
                }
                case "deathmatch arena":
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/Deathmatch/DeathmatchArena");
                    break;
                }
                default:
                {
                    SceneManager.LoadScene("GameScene");
                    break;
                }
            }
        }
    }

    private void InitLobbyData()
    {
        var childCount = playerContainer.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
        {
            Destroy(playerContainer.GetChild(i).gameObject);
        }

        var gameInfo = GameData.Instance.GameInfo;
        
        foreach (var player in gameInfo.Lobby().players)
        {
            var lobbyOwner = player.sessionId == gameInfo.Lobby().lobbyOwnerId;
            AddPlayer(player, lobbyOwner);

            if (player.sessionId == socketIOController.SocketID)
            {
                ShowLobbyLeaderObjects(lobbyOwner);
            }
        }
    }

    private void AddPlayer(Player player, bool lobbyOwner)
    {
        var playerObject = Instantiate(playerPrefab, playerContainer);

        var text = playerObject.GetComponentInChildren<Text>();
        text.text = player.name;

        if (lobbyOwner)
        {
            text.fontStyle = FontStyle.BoldAndItalic;
        }
    }

    public void StartGame()
    {
        var options = getGameStartParam();
        options.gameMode = getSelectedGameMode();


        socketIOController.Emit("start", JsonUtility.ToJson(options));
    }

    private string getSelectedGameMode()
    {
        var gameModeValue = gameModesDropdown.value;

        if (gameModeValue == 0)
        {
            throw new Exception("Choose correct game mode");
        }
        
        return _gameModes[gameModeValue];
    }

    private void ShowLobbyLeaderObjects(bool active)
    {
        foreach (var gameObject in lobbyLeaderObjects)
        {
            gameObject.SetActive(active);
        }
    }
    
}
