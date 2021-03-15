using System;
using System.Net.Http;
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
            var path = APIUrl + "/game/init";
            var param = new
            {
                timeLimit = 3600,
                singlePlayer= 1,
                playingAlone= 1
            };

            var result = PostRequest(path, param);

            var game = JsonUtility.FromJson<Game>(result);

            Game = game;

            return game;
        }

        public Game StartGame()
        {
            CheckIfGameStarted();

            var uuid = Game.uuid;

            var path = APIUrl + "/game/" + uuid + "/start";

            var result = PostRequest(path, new {});

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
                card.id
            };

            PostRequest(path, param);
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
