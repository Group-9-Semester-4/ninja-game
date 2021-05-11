using System;
using System.Collections.Generic;
using API;
using API.Models;
using API.Params;
using Game;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiscardScene : DiscardCardsScript
{
    void Start()
    {
        base.Start();
    }

    public void Continue()
    {
        GameService.Instance.StartGame(getCards(), getDiscardedCards(), getSelectedCardSet().id);

        var init = new GameInitParam();

        StartCoroutine(APIClient.Instance.InitGame(init, StartGame));
    }

    private void StartGame(API.Models.Game game)
    {
        GameService.Instance.game = game;
        SceneManager.LoadScene("GameScene");
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
