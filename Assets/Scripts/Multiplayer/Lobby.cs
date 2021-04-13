using System;
using System.Collections.Generic;
using API;
using API.Models;
using Game;
using SocketIOClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : DiscardCardsScript
{
    public GameObject playerPrefab;
    public Transform playerContainer;
    public Dropdown gameModesDropdown;

    public bool reloadLobby;
    public bool startGame;

    private SocketIO _socketIO;
    private List<string> _gameModes;
    
    void Start()
    {
        base.Start();

        _socketIO = SocketClient.Client;
        
        _socketIO.On("lobby-update", response =>
        {
            GameData.Instance.GameInfo = response.GetValue<GameInfo>();

            reloadLobby = true;
        });
        
        _socketIO.On("start", response =>
        {
            GameData.Instance.GameInfo = response.GetValue<GameInfo>();

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
            var gameMode = GameData.Instance.GameInfo.gameModeId;

            switch (gameMode)
            {
                case "basic":
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/BasicGame");
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
        
        foreach (var player in gameInfo.lobby.players)
        {
            var lobbyOwner = player.sessionId == gameInfo.lobby.lobbyOwnerId;
            AddPlayer(player, lobbyOwner);
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


        _socketIO.EmitAsync("start", options);
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
}
