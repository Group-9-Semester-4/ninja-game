using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public BossHealth bossHealth;

    public void FinishGame()
    {
        SceneManager.LoadScene("FinishScene");
    }
}

