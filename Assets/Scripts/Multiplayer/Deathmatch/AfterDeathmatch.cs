using System.Linq;
using API.Models.GameModes;
using Game;
using UnityEngine;

public class AfterDeathmatch : MonoBehaviour
{
    public GameObject playerScorePrefab;
    public Transform playerScoreContainer;
    
    void Start()
    {
        LoadLeaderBoard();
    }

    
    public void LoadLeaderBoard()
    {
        var childCount = playerScoreContainer.childCount;
        
        for (var i = 0; i < childCount; i++)
        {
            Destroy(playerScoreContainer.GetChild(i).gameObject);
        }
        
        var gameInfo = (DeathmatchGameModeGameInfo) GameData.Instance.GameInfo;
        
        var gameModeData = gameInfo.GameModeData();
        
        var myList = gameModeData.playerScores.OrderByDescending(score=>score.score).ToList();

        var playerList = gameModeData.players;

        var position = 1;
        
        foreach (var playerScore in myList)
        {
            var player = playerList.Find(playerEl => playerEl.sessionId == playerScore.playerId);

            var playerObject = Instantiate(playerScorePrefab, playerScoreContainer);

            var script = playerObject.GetComponentInChildren<PlayerScoreScript>();

            script.Init(position, player.name, playerScore.score);

            playerList.Remove(player);

            position++;
        }
        
    }
}
