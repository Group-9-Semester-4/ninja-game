using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIClient.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace APIClient
{
    public class APIClient : APIClientBase
    {
        // Singleton code
        private static APIClient _instance;
        private APIClient() {}
        public static APIClient Instance
        {
            get { return _instance ??= new APIClient(); }
        }


        // TODO: Change to dynamic env
        public const string APIUrl = "http://localhost:8080";

        public GameResource GameResource;

        public IEnumerator InitGame(GameInitOptions options, Action<GameResource> action)
        {
            const string path = APIUrl + "/game/init";

            var request = PostRequest(path, options);

            yield return HandleRequest(request);
        
            var game = JsonUtility.FromJson<GameResource>(request.downloadHandler.text);

            GameResource = game;

            action(game);
        }

        public IEnumerator StartGame(List<CardResource> unwantedCards, Action<GameResource> action)
        {
            CheckIfGameStarted();

            var uuid = GameResource.id;

            var path = APIUrl + "/game/" + uuid + "/start";

            var cardIds = unwantedCards.Select(card => card.id).ToArray();

            var request = PostRequest(path, cardIds);

            yield return HandleRequest(request);
            
            var game = JsonUtility.FromJson<GameResource>(request.downloadHandler.text);

            GameResource = game;

            action(game);
        }

        public IEnumerator DrawCard(Action<CardResource> action)
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.id;
            var path = APIUrl + "/game/" + uuid + "/draw";

            var request = GetRequest(path);

            yield return HandleRequest(request);
            
            var card = JsonUtility.FromJson<CardResource>(request.downloadHandler.text);

            action(card);
        }

        public IEnumerator CardDone(CardResource cardResource, Action action)
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.id;
            var path = APIUrl + "/game/" + uuid + "/done";
            
            var param = new
            {
                cardId = cardResource.id
            };

            var request = PostRequest(path, param);

            yield return HandleRequest(request);

            action();
        }

        public IEnumerator FinishGame(Action action)
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.id;
            var path = APIUrl + "/game/" + uuid + "/done";

            yield return PostRequest(path, new {});

            GameResource = null;

            action();
        }

        protected void CheckIfGameStarted()
        {
            if (GameResource == null)
            {
                throw new NullReferenceException("Init game before drawing a card");
            }
        }
        
        private IEnumerator HandleRequest(UnityWebRequest request)
        {
            request.SendWebRequest();
        
            while (!request.isDone)
            {
                yield return 0;
            }
        
            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception("API request " + request.url + "failed with status code " + request.responseCode);
            }
        }
    }
}
