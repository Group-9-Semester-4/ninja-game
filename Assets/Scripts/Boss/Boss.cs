using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public BossHealth bossHealth;

    public void FinishGame()
    {
        // Game mode separate data here

        if (GameData.Instance.IsMultiplayer)
        {
            var gameMode = GameData.Instance.GameInfo.gameModeId;
            switch (gameMode)
            {
                case "basic":
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/Basic/AfterBoss");
                    break;
                }
            }
            return;
        }

        SceneManager.LoadScene("FinishScene");
    }
}

