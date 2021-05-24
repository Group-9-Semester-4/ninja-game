using API.Models;
using UnityEngine;
using UnityEngine.UI;

public class DeathmatchCard : MonoBehaviour
{
    public WebReq webReq;
    
    public Text cardTitle;
    public Image containerImage;
    
    public Card card;

    public void Init(Card card)
    {
        this.card = card;
        cardTitle.text = card.name;
        
        webReq.card = card;
        
        Unlock();
        webReq.RenderCard();
    }

    public void Lock()
    {
        containerImage.color = Color.red;
    }

    public void Unlock()
    {
        containerImage.color = Color.white;
    }
}
