using System.Collections.Generic;
using API;
using API.Models;
using API.Params;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiscardCardsScript : MonoBehaviour
{
    public Transform CardContainer;
    public GameObject cardPrefab;
    
    // Start is called before the first frame update
    void Start()
    {

        var routine = APIClient.Instance.GetAllCards(PopulateCards);

        StartCoroutine(routine);
    }

    private void PopulateCards(List<Card> cards)
    {
        var y = 0;

        foreach (var card in cards)
        {
            var newGameObject = Instantiate(cardPrefab, CardContainer);
            
            var transformPosition = newGameObject.transform.position;
            transformPosition.y -= y;
            newGameObject.transform.position = transformPosition;

            var text = newGameObject.GetComponentInChildren<Text>();
            text.text = card.name;

            newGameObject.AddComponent(typeof(CardScript));
            var cardScript = (CardScript) newGameObject.GetComponent(typeof(CardScript));
            cardScript.card = card;
            
            y += 1;
        }
    }

    public void Continue()
    {
        var toggles = CardContainer.GetComponentsInChildren<Toggle>();

        var excludedCards = new List<string>();

        foreach (var toggle in toggles)
        {
            if (toggle.isOn)
            {
                var cardScript = (CardScript) toggle.GetComponent(typeof(CardScript));
                excludedCards.Add(cardScript.card.id);
            }
        }

        var options = new GameStartParam {unwantedCards = excludedCards};

        StartCoroutine(APIClient.Instance.StartGame(options, resource =>
        {
            SceneManager.LoadScene("GameScene");
        }));

    }
}

