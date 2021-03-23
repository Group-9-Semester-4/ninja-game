using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using APIClient.Models;
using UnityEditor.UI;
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
        slider.minValue = 0;
        slider.value = 0;
        IEnumerable<CardResource> allCards = APIClient.APIClient.Instance.getAllCards();
        slider.maxValue = allCards.Count();
        loadingOperation = SceneManager.LoadSceneAsync("MainMenu");
        loadingOperation.allowSceneActivation = false;
        var pathToSave = Application.persistentDataPath;
        var i = 0;
        while (!loadingOperation.isDone)
        {
            CardResource card = allCards.ElementAt(i);
            File.WriteAllBytes(pathToSave+card.id, APIClient.APIClient.Instance.getCardImage(card.filepath));
            slider.value++;
            i++;
            // Image loading logic
            // send request to lopcalhost/card/all
            // from body, zobrat vsetky urls ako novy array
            // foreach url v array stiahnut img a ten niekam ulozit

            if (allCards.Count() == 0) loadingOperation.allowSceneActivation = true;
            yield return null;
        }
    }
}
