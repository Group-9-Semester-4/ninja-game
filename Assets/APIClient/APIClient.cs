using System;
using System.Collections.Generic;
using System.Linq;
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

        public GameResource InitGame()
        {
            const string path = APIUrl + "/game/init";
            var param = new GameInitOptions();

            var result = PostRequest(path, param);

            var game = JsonUtility.FromJson<GameResource>(result);

            GameResource = game;

            return game;
        }

        public GameResource StartGame(List<CardResource> unwantedCards)
        {
            CheckIfGameStarted();

            var uuid = GameResource.uuid;

            var path = APIUrl + "/game/" + uuid + "/start";

            var cardIds = unwantedCards.Select(card => card.id).ToArray();

            var result = PostRequest(path, cardIds);

            GameResource = JsonUtility.FromJson<GameResource>(result);

            return GameResource;
        }

        public CardResource DrawCard()
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.uuid;
            var path = APIUrl + "/game/" + uuid + "/draw";

            var result = GetRequest(path);

            var card = JsonUtility.FromJson<CardResource>(result);

            return card;
        }

        public void CardDone(CardResource cardResource)
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.uuid;
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
            
            var uuid = GameResource.uuid;
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
    }
}
