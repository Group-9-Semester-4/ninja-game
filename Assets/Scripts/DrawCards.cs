using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIClient.Models;
using Game;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DrawCards : MonoBehaviour
{
    public GameObject Card;
    public GameObject DrawCardArea;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    // Update is called once per frame

/*
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://i.pinimg.com/originals/9f/ce/f1/9fcef1014d0d405429dfd38a4bc7aeba.jpg");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
    */
        public void OnClick()
        {

            var card = APIClient.APIClient.Instance.DrawCard();
            GameData.Instance.CurrentCard = card;

            //var mySprite = Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

            GameObject completeCard = Instantiate(Card, new Vector3(0, 0, 0), Quaternion.identity);
            completeCard.transform.SetParent(DrawCardArea.transform, false);
            //completeCard.AddComponent<Image>();
            //completeCard.GetComponent<Image>().sprite = mySprite;

        }
     
}
