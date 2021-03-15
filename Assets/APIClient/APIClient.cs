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
        
        protected Game Game;

        public Game InitGame()
        {
            const string path = APIUrl + "/game/init";
            var param = new GameInitOptions();

            var result = PostRequest(path, param);

            var game = JsonUtility.FromJson<Game>(result);

            Game = game;

            return game;
        }

        public Game StartGame(List<Card> unwantedCards)
        {
            CheckIfGameStarted();

            var uuid = Game.uuid;

            var path = APIUrl + "/game/" + uuid + "/start";

            var cardIds = unwantedCards.Select(card => card.id).ToArray();

            var result = PostRequest(path, cardIds);

            Game = JsonUtility.FromJson<Game>(result);

            return Game;
        }

        public Card DrawCard()
        {
            CheckIfGameStarted();
            
            var uuid = Game.uuid;
            var path = APIUrl + "/game/" + uuid + "/draw";

            var result = GetRequest(path);

            var card = JsonUtility.FromJson<Card>(result);

            return card;
        }

        public void CardDone(Card card)
        {
            CheckIfGameStarted();
            
            var uuid = Game.uuid;
            var path = APIUrl + "/game/" + uuid + "/done";
            
            var param = new
            {
                cardId = card.id
            };

            PostRequest(path, param);
        }

        public void FinishGame()
        {
            CheckIfGameStarted();
            
            var uuid = Game.uuid;
            var path = APIUrl + "/game/" + uuid + "/done";

            PostRequest(path, new {});

            Game = null;
        }

        protected void CheckIfGameStarted()
        {
            if (Game == null)
            {
                throw new NullReferenceException("Init game before drawing a card");
            }
        }
    }
}
