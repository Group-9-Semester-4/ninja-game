using System;
using System.Collections.Generic;
using System.IO;
using API.Models;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class GameService
    {
        private static GameService _instance;
        public static GameService Instance => _instance ??= new GameService();

        private List<Card> _cards;

        static Random _random = new Random();

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

        public void StartGame(List<Card> cards)
        {
            _cards = cards;
        }

        public Card DrawCard()
        {
            var r = _random.Next(_cards.Count);

            return _cards[r];
        }

        public void CardDone(Card card)
        {
            _cards.Remove(card);
        }

        public List<Card> remainingCards()
        {
            return _cards;
        }
    }
}