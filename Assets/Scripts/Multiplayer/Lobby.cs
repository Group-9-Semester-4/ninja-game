using System;
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

    public bool reloadLobby;
    public bool startGame;

    private SocketIO _socketIO;
    
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
            SceneManager.LoadScene("GameScene");
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
        
        foreach (var player in gameInfo.players)
        {
            var lobbyOwner = player.sessionId == gameInfo.lobbyOwnerId;
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

        _socketIO.EmitAsync("start", options);
    }
}
