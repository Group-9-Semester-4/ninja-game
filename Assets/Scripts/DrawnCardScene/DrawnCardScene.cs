using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DrawnCardScene : MonoBehaviour
{
    public GameObject timer;
    public Text timerText;
    public float timeLeft;
    public bool timerStarted;
    public bool timerFinished;

    public WebReq webReq;

    public void Start()
    {
        // Get webReq reference
        GameObject findObject = GameObject.Find("GameManagerObject");
        webReq = findObject.GetComponent<WebReq>();
        
        // Show timer stuff when card is timer related
        var currentCard = GameData.Instance.CurrentCard;
        
        if (currentCard.difficulty_type)
        {
            ShowTimer(currentCard.difficulty);
        }

        webReq.cardResource = currentCard;
        webReq.RenderCard();
    }
    
    void Update()
    {
        if (!timerFinished && timerStarted)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = (timeLeft).ToString("0");
            if (timeLeft < 0)
            {
                timerFinished = true;
            }
        }

        if (timerFinished)
        {
            // Timer was finished start blinking time left
            timerText.text = Time.fixedTime % .5 < .2 ? "" : "0";
        }
    }

    public void RedrawCard()
    {
        var card = APIClient.APIClient.Instance.DrawCard();

        if (card.difficulty_type)
        {
            ShowTimer(card.difficulty);
        }
        else
        {
            HideTimer();
        }

        webReq.cardResource = card;
        webReq.RenderCard();
        Debug.Log(webReq.cardResource.name);
    }
    
    public void CompleteCard()
    {
        var gameData = GameData.Instance;

        if (gameData.CurrentCard != null)
        {
            gameData.Points += gameData.CurrentCard.points;
            gameData.CurrentCard = null;
        }

        SceneManager.LoadScene("GameScene");
    }

    public void StartTimer()
    {
        timerStarted = true;
    }
    
    private void ShowTimer(int seconds)
    {
        timerStarted = false;
        timerFinished = false;
        
        timeLeft = seconds;
        timer.SetActive(true);
        timerText.text = (timeLeft).ToString("0");
    }

    private void HideTimer()
    {
        timer.SetActive(false);
    }
}
