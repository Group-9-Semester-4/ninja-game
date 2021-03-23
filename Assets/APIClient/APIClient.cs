using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APIClient.Models;
using UnityEngine;

namespace APIClient
{
    public class APIClient : APIClientBase
    {
        // Singleton code
        private static APIClient _instance;
        private APIClient() {}
        public static APIClient Instance
        {
            get { return _instance ?? (_instance = new APIClient()); }
        }


        // TODO: Change to dynamic env
        protected const string APIUrl = "http://localhost:8080";

        public GameResource GameResource;

        public GameResource InitGame(GameInitOptions options)
        {
            const string path = APIUrl + "/game/init";

            var result = PostRequest(path, options);

            var game = JsonUtility.FromJson<GameResource>(result);

            GameResource = game;

            return game;
        }

        // Used for testing purposes, use when you need to start a game, and don't want to go through all the screens
        public void TestInit()
        {
            InitGame(new GameInitOptions());
            StartGame(new List<CardResource>());
        }

        public GameResource StartGame(List<CardResource> unwantedCards)
        {
            CheckIfGameStarted();

            var uuid = GameResource.id;

            var path = APIUrl + "/game/" + uuid + "/start";

            var cardIds = unwantedCards.Select(card => card.id).ToArray();

            var result = PostRequest(path, cardIds);

            GameResource = JsonUtility.FromJson<GameResource>(result);

            return GameResource;
        }

        public CardResource DrawCard()
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.id;
            var path = APIUrl + "/game/" + uuid + "/draw";

            var result = GetRequest(path);

            var card = JsonUtility.FromJson<CardResource>(result);

            return card;
        }

        public void CardDone(CardResource cardResource)
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.id;
            var path = APIUrl + "/game/" + uuid + "/done";
            
            var param = new
            {
                cardId = cardResource.id
            };

            PostRequest(path, param);
        }

        public void FinishGame()
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.id;
            var path = APIUrl + "/game/" + uuid + "/done";

            PostRequest(path, new {});

            GameResource = null;
        }

        protected void CheckIfGameStarted()
        {
            if (GameResource == null)
            {
                throw new NullReferenceException("Init game before drawing a card");
            }
        }

        public IEnumerable<CardResource> getAllCards()
        {
            var path = APIUrl + "/card" + "/all";
            var response = GetRequest(path);
            response = "{\"Items\":" + response + "}";
            return JsonHelper.FromJson<CardResource>(response);

        }

        public byte[] getCardImage(String filepath)
        {
            var responseImage = GetRequest(filepath);
            return Encoding.ASCII.GetBytes(responseImage);
        }
    }
}
