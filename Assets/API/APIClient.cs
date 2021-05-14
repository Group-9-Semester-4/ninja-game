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
        
        public IEnumerator InitGame(GameInitParam param, Action<Models.Game> action)
        {
            const string path = APIUrl + "/game/init";

            var request = PostRequest(path, param);

            yield return HandleRequest(request);
        
            var game = JsonUtility.FromJson<Models.Game>(request.downloadHandler.text);

            action(game);
        }
        
        public IEnumerator FinishGame(FinishGameParam param, Action action)
        {
            const string path = APIUrl + "/game/finish";
            
            var request = PostRequest(path, param);

            yield return HandleRequest(request);

            action();
        }

        public IEnumerator GetAllCards(Action<List<Card>> action)
        {
            var path = APIUrl + "/game/cards";

            var request = GetRequest(path);

            yield return HandleRequest(request);

            var cards = JsonHelper.FromJson<Card>(request.downloadHandler.text);

            action(cards.ToList());
        }

        public IEnumerator GetCardSets(Action<List<CardSet>> action)
        {
            var path = APIUrl + "/game/cardsets";

            var request = GetRequest(path);

            yield return HandleRequest(request);

            var cardSets = JsonHelper.FromJson<CardSet>(request.downloadHandler.text);

            action(cardSets.ToList());
        }

        public IEnumerator GetGameModes(Action<string[]> action)
        {
            var path = APIUrl + "/game/game-modes";

            var request = GetRequest(path);

            yield return HandleRequest(request);

            var gameModes = JsonHelper.FromJson<string>(request.downloadHandler.text);

            action(gameModes);
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
