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
}