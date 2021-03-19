using System;
using System.Collections;
using APIClient.Models;
using UnityEngine;
using UnityEngine.Networking;

public class WebReq : MonoBehaviour
{
    //[SerializeField] public TextMeshPro textMesh;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public CardResource cardResource;

    private void GetTexture(string url, Action<string> onError, Action<Texture2D> onSucces)
    {
        StartCoroutine(GetTextureCoroutine(url, onError, onSucces));

    }
    private IEnumerator GetTextureCoroutine(string url, Action<string> onError, Action<Texture2D> onSucces)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError )
            {
                onError(unityWebRequest.error);
                //

            } else
            {
                DownloadHandlerTexture downloadHandlerTexture = unityWebRequest.downloadHandler as DownloadHandlerTexture;
                onSucces(downloadHandlerTexture.texture);
                Debug.Log("Recieved: " + unityWebRequest.downloadHandler.text);
            }
        }
    }



    //_______________________________________________________________________
    private void Get(string url, Action<string> onError, Action<string> onSucces)
    {
        StartCoroutine(GetCoroutine(url, onError, onSucces));

    }

    private IEnumerator GetCoroutine(string url, Action<string> onError, Action<string> onSucces)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                onError(unityWebRequest.error);
                //

            }
            else
            {
                onSucces(unityWebRequest.downloadHandler.text);
                //Debug.Log("Recieved: " + unityWebRequest.downloadHandler.text);
            }
        }
    }

    public void RenderCard()
    {
        GetTexture(cardResource.filepath, (string error) => {
            //error
            Debug.Log("Error: " + error);
            //textMesh.SetText("Error: " + error);
        }, (Texture2D texture2D) => {
            //succesfully contacted URL
            //textMesh.SetText("Success!");
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(.5f, .5f), 100.0f);
            spriteRenderer.sprite = sprite;

        });
    }
}


