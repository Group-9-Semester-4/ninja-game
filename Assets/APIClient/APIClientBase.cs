using System;
using UnityEngine;
using UnityEngine.Networking;

namespace APIClient
{
    public class APIClientBase
    {
        protected UnityWebRequest GetRequest(string url)
        {
            var request = UnityWebRequest.Get(url);

            return request;
        }

        protected UnityWebRequest PostRequest(string url, object param)
        {
            var serialized = JsonUtility.ToJson(param);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(serialized);
            
            var request = new UnityWebRequest(url, "POST");

            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }
    }
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}