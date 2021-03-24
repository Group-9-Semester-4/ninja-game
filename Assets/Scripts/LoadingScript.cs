using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using APIClient.Models;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public Slider slider;

    private int _done;
    private string _imageSavePath;

    private Thread _imageLoadingThread;
    
    void Start()
    {
        _imageSavePath = GameService.Instance.ImageSavePath();
        
        var allCards = APIClient.APIClient.Instance.GetCards().ToList();

        slider.minValue = 0;
        slider.maxValue = allCards.Count;
        slider.value = 0;

        var imageThreadStart = new ThreadStart(() => LoadImages(allCards));
        _imageLoadingThread = new Thread(imageThreadStart);

        _imageLoadingThread.Start();
        
    }

    private void Update()
    {
        slider.value = _done;

        if (!_imageLoadingThread.IsAlive)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void LoadImages(IEnumerable<CardResource> allCards)
    {
        foreach (var card in allCards)
        {
            if (!Directory.Exists(_imageSavePath))
            {
                Directory.CreateDirectory(_imageSavePath);
            }

            var localFilePath = _imageSavePath + card.id;

            if (!File.Exists(localFilePath))
            {
                var content = APIClient.APIClient.Instance.DownloadImage(card.filepath);
                File.WriteAllBytes(localFilePath, content);
            }

            _done++;
        }
    }
}
