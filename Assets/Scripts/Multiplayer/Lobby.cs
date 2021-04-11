using System;
using API;
using API.Models;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : DiscardCardsScript
{
    public GameObject playerPrefab;
    public Transform playerContainer;

    public bool reloadLobby;
    
    void Start()
    {
        base.Start();
        
        SocketClient.Client.On("join", response =>
        {
            GameData.Instance.GameInfo = response.GetValue<GameInfo>();

            reloadLobby = true;
        });
        
        SocketClient.Client.On("leave", response =>
        {
            GameData.Instance.GameInfo = response.GetValue<GameInfo>();

            reloadLobby = true;
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
    }

    private void InitLobbyData()
    {
        var childCount = playerContainer.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
        {
            Destroy(playerContainer.GetChild(i).gameObject);
        }
        
        foreach (var player in GameData.Instance.GameInfo.players)
        {
            AddPlayer(player);
        }
    }

    private void AddPlayer(Player player)
    {
        var playerObject = Instantiate(playerPrefab, playerContainer);

        var text = playerObject.GetComponentInChildren<Text>();
        text.text = player.name;
    }

    public void StartGame()
    {
        var options = getGameStartParam();

        StartCoroutine(APIClient.Instance.StartGame(options, resource =>
        {
            SceneManager.LoadScene("GameScene");
        }));
    }
}
