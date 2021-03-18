using System.Collections;
using System.Collections.Generic;
using APIClient.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        APIClient.APIClient.Instance.InitGame();
        APIClient.APIClient.Instance.StartGame(new List<Card>());
    }
    //sceneloader for changing scenes

    public void SceneLoader(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        //Debug.Log(SceneName);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
