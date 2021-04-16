using API;
using API.Models;
using API.Models;
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

    public Text cardDescription;
    public Text cardRepetitions;

    public WebReq webReq;

    public Card currentCard;

    public void Start()
    {
        // Show timer stuff when card is timer related
        currentCard = GameData.Instance.CurrentCard;
        
        if (currentCard.hasTimer)
        {
            ShowTimer(currentCard.difficulty);
        }

        SetCardInfo(currentCard);

        webReq.card = currentCard;
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
        HideCurrentCard();
        StartCoroutine(APIClient.Instance.DrawCard(card =>
        {
            GameData.Instance.CurrentCard = card;
            SetCardInfo(card);
            RenderCard();
        }));
    }

    public void RenderCard()
    {
        currentCard = GameData.Instance.CurrentCard;
        HideTimer();
        Debug.Log(currentCard.hasTimer);
        if (currentCard.hasTimer)
        {
            ShowTimer(currentCard.difficulty);
        }

        webReq.card = currentCard;
        webReq.RenderCard();
    }

    public void HideCurrentCard()
    {
        webReq.HideCard();
    }
    
    public void CompleteCard()
    {
        var gameData = GameData.Instance;

        if (gameData.CurrentCard != null)
        {
            gameData.Points += gameData.CurrentCard.points;
        }

        var routine = APIClient.Instance.CardDone(gameData.CurrentCard, () =>
        {
            SceneManager.LoadScene("GameScene");
        });

        StartCoroutine(routine);
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

    private void SetCardInfo(Card card)
    {
        cardDescription.text = card.description;

        if (card.hasTimer)
        {
            cardRepetitions.text = card.difficulty + " seconds";
        }
        else
        {
            cardRepetitions.text = card.difficulty + " repetitions";
        }
    }
}