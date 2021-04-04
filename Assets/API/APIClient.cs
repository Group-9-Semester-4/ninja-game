using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using API.Models;
using API.Params;
using UnityEngine;
using UnityEngine.Networking;

namespace API
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
        public const string APIUrl = "http://localhost:8080/api";

        public GameResource GameResource;

        public IEnumerator InitGame(GameInitParam param, Action<GameResource> action)
        {
            const string path = APIUrl + "/game/init";

            var request = PostRequest(path, param);

            yield return HandleRequest(request);
        
            var game = JsonUtility.FromJson<GameResource>(request.downloadHandler.text);

            GameResource = game;

            action(game);
        }

        public IEnumerator StartGame(GameStartParam param, Action<GameResource> action)
        {
            CheckIfGameStarted();
            
            const string path = APIUrl + "/game/start";
            
            param.gameId = GameResource.id;

            var request = PostRequest(path, param);

            yield return HandleRequest(request);
            
            var game = JsonUtility.FromJson<GameResource>(request.downloadHandler.text);

            GameResource = game;

            action(game);
        }

        public IEnumerator DrawCard(Action<CardResource> action)
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.id;
            
            var path = APIUrl + "/game/draw?gameId="+uuid;

            var request = GetRequest(path);

            yield return HandleRequest(request);
            
            var card = JsonUtility.FromJson<CardResource>(request.downloadHandler.text);

            action(card);
        }

        public IEnumerator CardDone(CardResource cardResource, Action action)
        {
            CheckIfGameStarted();
            
            const string path = APIUrl + "/game/card-done";
            
            var uuid = GameResource.id;

            var options = new CardDoneParam {cardId = cardResource.id, gameId = uuid};

            var request = PostRequest(path, options);

            yield return HandleRequest(request);

            action();
        }

        public IEnumerator FinishGame(Action action)
        {
            CheckIfGameStarted();
            
            const string path = APIUrl + "/game/finish";

            var param = new FinishGameParam() {gameId = GameResource.id};

            yield return PostRequest(path, param);

            GameResource = null;

            action();
        }

        public IEnumerator GetAllCards(Action<List<CardResource>> action)
        {
            CheckIfGameStarted();

            var path = APIUrl + "/game/cards";

            var request = GetRequest(path);

            yield return HandleRequest(request);

            var cards = JsonHelper.FromJson<CardResource>(request.downloadHandler.text);

            action(cards.ToList());
        }

        protected void CheckIfGameStarted()
        {
            if (GameResource == null)
            {
                throw new NullReferenceException("Game not initialized");
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
