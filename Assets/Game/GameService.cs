using System;
using System.Collections.Generic;
using System.IO;
using API.Models;
using API.Params;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class GameService
    {
        private static GameService _instance;
        public static GameService Instance => _instance ??= new GameService();

        public static GameService Reinstantiate()
        {
            _instance = new GameService();
            return _instance;
        }

        private List<Card> _cards;
        private Card lastDrawnCard;
        private int started;
        private List<string> listOfRedrawnCards = new List<string>();
        private string cardSetId;
        private List<string> discardedCards = new List<string>();
        
        public API.Models.Game game;
        
        public int score;
        public int cardsCompleted;
        public int successfulHits;
        public int ammo;

        private static readonly Random Random = new Random();

        public byte[] GetCardImage(Card card)
        {
            var id = card.id;

            var folder = ImageSavePath();
            var filePath = folder + id;

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }

            throw new NullReferenceException("File not cached");
        }

        public string ImageSavePath()
        {
            return Application.persistentDataPath + "/card-images/";
        }

        public void StartGame(List<Card> cards, List<Card> discardedCards, string cardSetId)
        {
            this.cardSetId = cardSetId;

            foreach (var card in discardedCards)
            {
                this.discardedCards.Add(card.id);
            }
            
            started = Epoch.Current();
            _cards = cards;
        }

        public Card DrawCard(bool redraw = false)
        {
            if (redraw)
            {
                if (lastDrawnCard != null)
                {
                    listOfRedrawnCards.Add(lastDrawnCard.id);
                }
            }
            
            var r = Random.Next(_cards.Count);
            Card card = _cards[r];
            if (_cards.Count == 1)
            {
                return card;
            }
            
            while (card == lastDrawnCard)
            {
                r = Random.Next(_cards.Count);
                card = _cards[r];
            }

            lastDrawnCard = card;
            return card;
        }

        public void CardDone(Card card)
        {
            cardsCompleted++;
            _cards.Remove(card);
        }

        public List<Card> remainingCards()
        {
            return _cards;
        }

        public FinishGameParam FinishGameParam()
        {
            var timeElapsed = Epoch.Current() - started;

            var param = new FinishGameParam
            {
                gameId = game.id,
                cardSetId = cardSetId,
                timeInSeconds = timeElapsed,
                cardsCompleted = cardsCompleted,
                unwantedCards = discardedCards,
                listOfRedrawnCards = listOfRedrawnCards
            };

            return param;
        }
    }
    
    public static class Epoch {
 
        public static int Current()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
 
            return currentEpochTime;
        }
 
        public static int SecondsElapsed(int t1)
        {
            int difference = Current() - t1;
 
            return Mathf.Abs(difference);
        }
 
        public static int SecondsElapsed(int t1, int t2)
        {
            int difference = t1 - t2;
 
            return Mathf.Abs(difference);
        }
 
    }
}