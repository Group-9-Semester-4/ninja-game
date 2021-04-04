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
    public Dropdown CardSetDropdown;
    public GameObject cardPrefab;

    private List<CardSet> _cardSets;
    
    void Start()
    {
        _cardSets = new List<CardSet>() { new CardSet() {cards = new List<Card>()} };

        var cardSetRoutine = APIClient.Instance.GetCardSets(PopulateCardSets);

        StartCoroutine(cardSetRoutine);
    }

    private void PopulateCards(List<Card> cards)
    {
        var childCount = CardContainer.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
        {
            Destroy(CardContainer.GetChild(i).gameObject);
        }
        
        var y = 0;
        
        Debug.Log("Populating "+ cards.Count);

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
            
            Debug.Log(SceneManager.GetActiveScene().name);
            
            // TODO: Fix scale after all screens are fixed
            if (SceneManager.GetActiveScene().name == "DiscardScene")
            {
                y += 1;
            }
            else
            {
                y += 50;
            }
        }
    }

    private void PopulateCardSets(List<CardSet> cardSets)
    {
        foreach (var cardSet in cardSets)
        {
            _cardSets.Add(cardSet);
            
            var optionData = new Dropdown.OptionData(cardSet.name);
            CardSetDropdown.options.Add(optionData);
        }
    }

    private CardSet getSelectedCardSet()
    {
        var index = CardSetDropdown.value;

        return _cardSets[index];
    }

    public void onCardSetChange()
    {
        var cardSet = getSelectedCardSet();

        PopulateCards(cardSet.cards);
    }

    public void Continue()
    {
        var cardSet = getSelectedCardSet();

        if (string.IsNullOrEmpty(cardSet.id))
        {
            return;
        }
        
        var toggles = CardContainer.GetComponentsInChildren<Toggle>();

        var excludedCards = new List<string>();

        foreach (var toggle in toggles)
        {
            if (!toggle.isOn)
            {
                var cardScript = (CardScript) toggle.GetComponent(typeof(CardScript));
                excludedCards.Add(cardScript.card.id);
            }
        }

        var options = new GameStartParam {unwantedCards = excludedCards, cardSetId = cardSet.id};

        StartCoroutine(APIClient.Instance.StartGame(options, resource =>
        {
            SceneManager.LoadScene("GameScene");
        }));

    }
}

