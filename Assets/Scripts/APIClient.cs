using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class APIClient : MonoBehaviour
{
    // TODO: Change to dynamic env
    protected string APIUrl = "http://localhost:8080";
    
    public CardAPIResource DrawCard()
    {
        // TODO: Change to actual API loading
        
        var card = new CardAPIResource()
        {
            id = 1, 
            imageUrl = "https://i.pinimg.com/originals/9f/ce/f1/9fcef1014d0d405429dfd38a4bc7aeba.jpg"
        };

        return card;
    }
}
