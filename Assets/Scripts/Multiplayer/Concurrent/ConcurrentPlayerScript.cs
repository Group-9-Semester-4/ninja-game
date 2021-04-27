using API.Models;
using UnityEngine;
using UnityEngine.UI;

public class ConcurrentPlayerScript : MonoBehaviour
{
    public Text userName;
    public Text cardsDone;

    public void SetUserName(string name)
    {
        userName.text = name;
    }
    
    public void SetDoneAmount(int amount)
    {
        cardsDone.text = amount.ToString();
    }

}
