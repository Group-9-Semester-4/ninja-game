using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    //sceneloader for changing scenes

    public void SceneLoader(string SceneName)
    {
        Debug.Log(SceneName);
        SceneManager.LoadScene(SceneName);
        //Debug.Log(SceneName);
    }
}
