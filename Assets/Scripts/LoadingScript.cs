using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public Slider slider;

    private AsyncOperation loadingOperation;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingScreen());
    }

    IEnumerator LoadingScreen()
    {
        // Load slider info
        //slider.minValue = 0;
        //slider.maxValue = 1000;
        //slider.value = 1;

        loadingOperation = SceneManager.LoadSceneAsync("MainMenu");
        loadingOperation.allowSceneActivation = false;
        
        while (!loadingOperation.isDone)
        {
            // Image loading logic

            //loadingOperation.allowSceneActivation = true;
            yield return null;
        }
    }
}
