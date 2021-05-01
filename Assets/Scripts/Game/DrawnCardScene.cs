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
    public bool timerStopped;
    public bool timerFinished;
    
    public GameObject stopTimerButton;
    public GameObject startTimerButton;
    
    public Text cardDescription;
    public Text cardRepetitions;
    public GameObject completeButton;
    public GameObject redrawButton;

    public WebReq webReq;

    public Card currentCard;

    public void Start()
    {
        // Show timer stuff when card is timer related
        currentCard = GameData.Instance.CurrentCard;
        stopTimerButton.SetActive(false);
        if (currentCard.hasTimer)
        {
            ShowTimer(currentCard.difficulty);
        }
        //prevents from a bug - initial load of gamescene often had both timer & completed button
        else {
            HideTimer();
        }

        SetCardInfo(currentCard);

        webReq.card = currentCard;
        webReq.RenderCard();
    }
    
    void Update()
    {
        if (!timerFinished && timerStarted && !timerStopped)
        {
            startTimerButton.SetActive(false);
            stopTimerButton.SetActive(true);
            timeLeft -= Time.deltaTime;
            timerText.text = (timeLeft).ToString("0");
            if (timeLeft < 0)
            {
                stopTimerButton.SetActive(false);
                completeButton.SetActive(true);
                timer.SetActive(false);
                timerFinished = true;
            }
            else
            {
                redrawButton.SetActive(false);
                stopTimerButton.SetActive(true);
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
        completeButton.SetActive(true);
        HideCurrentCard();
        var card = GameService.Instance.DrawCard();
        
        GameData.Instance.CurrentCard = card;
        
        SetCardInfo(card);
        RenderCard();
    }

    public void RenderCard()
    {
        currentCard = GameData.Instance.CurrentCard;
        HideTimer();
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
        
        GameService.Instance.CardDone(gameData.CurrentCard);

        GameService.Instance.cardsCompleted++;
        
        SceneManager.LoadScene("GameScene");

    }

    public void StartTimer()
    {
        timerStarted = true;
        timerStopped = false;
    }

    public void StopTimer()
    {
        timerStopped = true;
        startTimerButton.SetActive(true);
        stopTimerButton.SetActive(false);
        completeButton.SetActive(false);
        redrawButton.SetActive(true);
    }
    
    private void ShowTimer(int seconds)
    {
        completeButton.SetActive(false);
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

    public void FinishGame()
    {
        SceneManager.LoadScene("FinishScene");
    }
}