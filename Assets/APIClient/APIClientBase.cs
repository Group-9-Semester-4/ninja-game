using System.Net.Http;
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