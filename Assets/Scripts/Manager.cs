using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    //sceneloader for changing scenes

    public void SceneLoader(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
