using UnityEngine;

public class DrawCard : MonoBehaviour
{
    public void OnClick()
    {
        GameObject findObject = GameObject.Find("GameManagerObject");
        WebReq webReq = findObject.GetComponent<WebReq>();

        webReq.RenderNewCard();
        Debug.Log(webReq.cardResource.name);
    }
}
