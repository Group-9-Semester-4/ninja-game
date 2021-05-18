using UnityEngine;
using System.Linq;
using API;
using API.Models;
using API.Models.GameModes;
using Game;

public class AfterConcurrentGame : MonoBehaviour
{
    public GameObject playerScorePrefab;
    public Transform playerScoreContainer;

    private bool _reload;
    
    void Start()
    {
        var socketIo = SocketClient.Client;
        
        socketIo.On("boss-score-update", response =>
        {
            GameData.Instance.GameInfo = JsonUtility.FromJson<GameInfo>(response.data);
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
        
        for (var i = 0; i < childCount; i++)
        {
            Destroy(playerScoreContainer.GetChild(i).gameObject);
        }
        
        var gameInfo = (ConcurrentGameModeGameInfo) GameData.Instance.GameInfo;
        
        var gameModeData = gameInfo.GameModeData();
        
        var myList = gameModeData.bossFightScores.OrderByDescending(score=>score.score).ToList();

        var playerList = gameModeData.players;

        var position = 1;
        
        foreach (var bossFightScore in myList)
        {
            var player = playerList.Find(playerEl => playerEl.sessionId == bossFightScore.playerId);

            AddScore(position, player.name, bossFightScore.score);

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
