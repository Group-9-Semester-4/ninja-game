using System.Collections.Generic;
using APIClient.Models;
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
        var allCards = APIClient.APIClient.Instance.GameResource.allCards;

        var y = 0;

        foreach (var card in allCards)
        {
            var newGameObject = Instantiate(cardPrefab, CardContainer);
            
            var transformPosition = newGameObject.transform.position;
            transformPosition.y -= y;
            newGameObject.transform.position = transformPosition;

            var text = newGameObject.GetComponentInChildren<Text>();
            text.text = card.name;

            newGameObject.AddComponent(typeof(CardScript));
            var cardScript = (CardScript) newGameObject.GetComponent(typeof(CardScript));
            cardScript.cardResource = card;
            
            y += 1;
        }
    }

    public void Continue()
    {
        var toggles = CardContainer.GetComponentsInChildren<Toggle>();

        var excludedCards = new List<CardResource>();

        foreach (var toggle in toggles)
        {
            if (toggle.isOn)
            {
                var cardScript = (CardScript) toggle.GetComponent(typeof(CardScript));
                excludedCards.Add(cardScript.cardResource);
            }
        }

        StartCoroutine(APIClient.APIClient.Instance.StartGame(excludedCards, resource =>
        {
            SceneManager.LoadScene("GameScene");
        }));

    }
}

