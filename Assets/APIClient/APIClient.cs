using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APIClient.Models;
using Game;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

        public IEnumerator InitGame(GameInitOptions options)
        {
            const string path = APIUrl + "/game/init";

            var request = PostRequest(path, options);

            yield return HandleRequest(request);
        
            var game = JsonUtility.FromJson<GameResource>(request.downloadHandler.text);

            GameResource = game;

            SceneManager.LoadScene("DiscardScene");
        }

        public IEnumerator StartGame(List<CardResource> unwantedCards)
        {
            CheckIfGameStarted();

            var uuid = GameResource.id;

            var path = APIUrl + "/game/" + uuid + "/start";

            var cardIds = unwantedCards.Select(card => card.id).ToArray();

            var request = PostRequest(path, cardIds);

            yield return HandleRequest(request);
            
            var game = JsonUtility.FromJson<GameResource>(request.downloadHandler.text);

            GameResource = game;

            SceneManager.LoadScene("GameScene");
        }

        public IEnumerator DrawCard()
        {
            yield return RedrawCard();
            
            SceneManager.LoadScene("DrawnCardScene");
        }
        
        public IEnumerator RedrawCard()
        {
            CheckIfGameStarted();
            
            var uuid = GameResource.id;
            var path = APIUrl + "/game/" + uuid + "/draw";

            var request = GetRequest(path);

            yield return HandleRequest(request);
            
            var card = JsonUtility.FromJson<CardResource>(request.downloadHandler.text);

            GameData.Instance.CurrentCard = card;
        }

        public IEnumerator CardDone(CardResource cardResource)
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
