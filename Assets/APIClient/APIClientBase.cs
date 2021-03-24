using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;

namespace APIClient
{
    public class APIClientBase
    {
        protected string GetRequest(string url)
        {
            var client = new HttpClient();
            
            var result = client.GetAsync(url);
            result.Wait();

            if (!result.Result.IsSuccessStatusCode)
            {
                throw new HttpRequestException("API request " + url + "failed with status code " + result.Result.StatusCode);
            }
            
            var content = result.Result.Content.ReadAsStringAsync();
            content.Wait();

            return content.Result;
        }

        protected string PostRequest(string url, object param)
        {
            var client = new HttpClient();
            
            var serialized = JsonUtility.ToJson(param);
            
            if (serialized == "{}")
            {
                serialized = "[]";
            }
            
            var jsonContent = new StringContent(serialized, Encoding.UTF8, "application/json");

            var result = client.PostAsync(url, jsonContent);
            result.Wait();

            if (!result.Result.IsSuccessStatusCode)
            {
                throw new HttpRequestException("API request " + url + "failed with status code " + result.Result.StatusCode);
            }
            
            var content = result.Result.Content.ReadAsStringAsync();
            content.Wait();

            return content.Result;
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