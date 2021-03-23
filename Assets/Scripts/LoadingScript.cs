using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using APIClient.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public Slider slider;

    private int done;
    private string imageSavePath;

    private Thread imageLoadingThread;
    
    void Start()
    {
        imageSavePath = Application.persistentDataPath + "/card-images/";
        
        var allCards = APIClient.APIClient.Instance.GetCards().ToList();

        slider.minValue = 0;
        slider.maxValue = allCards.Count;
        slider.value = 0;

        var imageThreadStart = new ThreadStart(() => LoadImages(allCards));
        imageLoadingThread = new Thread(imageThreadStart);

        imageLoadingThread.Start();
        
    }

    private void Update()
    {
        slider.value = done;

        if (!imageLoadingThread.IsAlive)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void LoadImages(List<CardResource> allCards)
    {
        foreach (var card in allCards)
        {
            if (!Directory.Exists(imageSavePath))
            {
                Directory.CreateDirectory(imageSavePath);
            }

            var localFilePath = imageSavePath + card.id;

            if (!File.Exists(localFilePath))
            {
                var content = APIClient.APIClient.Instance.DownloadImage(card.filepath);
                File.WriteAllBytes(localFilePath, content);
            }

            done++;
        }
    }
}
