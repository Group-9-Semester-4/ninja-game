using System;
using System.Net.Http;
using APIClient.Models;
using UnityEngine;

namespace APIClient
{
    public class APIClient
    {
        // Singleton code
        private static APIClient _instance;
        private APIClient() {}
        public static APIClient Instance => _instance ?? (_instance = new APIClient());
        
        
        // TODO: Change to dynamic env
        protected const string APIUrl = "http://localhost:8080";
        
        protected Game Game;

        public Card DrawCard()
        {
            // TODO: Change to actual API loading
        
            var card = new Card()
            {
                id = "1", 
                imageUrl = "https://i.pinimg.com/originals/9f/ce/f1/9fcef1014d0d405429dfd38a4bc7aeba.jpg"
            };

            return card;
        }

        public Game InitGame()
        {
            var path = APIUrl + "/game/init";
            var param = new
            {
                timeLimit = 360,
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
            if (Game == null)
            {
                throw new NullReferenceException("Init game before starting it");
            }

            var uuid = Game.uuid;

            var path = APIUrl + "/game/" + uuid + "/start";
            
            // TODO: Send request
            
            return Game;
        }

        protected string GetRequest(string url)
        {
            var client = new HttpClient();
            
            var result = client.GetAsync(url);
            result.Wait();

            var content = result.Result.Content.ReadAsStringAsync();
            content.Wait();

            return content.Result;
        }

        protected string PostRequest(string url, object param)
        {
            var client = new HttpClient();
            var jsonContent = new StringContent(JsonUtility.ToJson(param));

            var result = client.PostAsync(url, jsonContent);
            result.Wait();

            var content = result.Result.Content.ReadAsStringAsync();
            content.Wait();

            return content.Result;
        }
    }
}
