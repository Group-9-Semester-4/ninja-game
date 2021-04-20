using System.Linq;
using API;
using API.Models;
using API.Models.GameModes;
using Game;
using UnityEngine;

public class AfterBasicGame : MonoBehaviour
{

    public GameObject playerScorePrefab;
    public Transform playerScoreContainer;

    private bool _reload;
    
    void Start()
    {
        var socketIo = SocketClient.Client;
        
        socketIo.On("boss-score-update", response =>
        {
            GameData.Instance.GameInfo = response.GetValue<GameInfo>();
            _reload = true;
        });
        
        LoadLeaderBoard();
    }
    
    
    void Update()
    {
        if (_reload)
        {
            _reload = false;
            LoadLeaderBoard();
        }
    }

    public void LoadLeaderBoard()
    {
        var childCount = playerScoreContainer.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
        {
            Destroy(playerScoreContainer.GetChild(i).gameObject);
        }
        
        var gameInfo = GameData.Instance.GameInfo;
        
        var gameModeData = (BasicGameMode) gameInfo.gameModeData.ToObject(typeof(BasicGameMode));
        
        var myList = gameModeData.bossFightScores.ToList();

        myList.Sort((pair1,pair2) => pair1.Value.CompareTo(pair2.Value));

        var playerList = gameModeData.players;

        var position = 1;
        
        foreach (var pair in myList)
        {
            var player = playerList.Find(playerEl => playerEl.sessionId == pair.Key);

            AddScore(position, player.name, pair.Value);

            playerList.Remove(player);

            position++;
        }


        foreach (var player in playerList)
        {
            AddScore(position, player.name, 0);

            playerList.Remove(player);

            position++;
        }
    }

    private void AddScore(int position, string name, int score)
    {
        var playerObject = Instantiate(playerScorePrefab, playerScoreContainer);

        var script = playerObject.GetComponentInChildren<PlayerScoreScript>();

        script.Init(position, name, score);
    }
    
}
