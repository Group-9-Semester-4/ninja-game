using API.Models;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Player player;
    public SpriteRenderer statusColor;
    public Text userName;

    void Start()
    {
        userName.text = player.name;
    }

    public void setOnTurn()
    {
        statusColor.color = Color.yellow;
    }

    public void setComplete()
    {
        statusColor.color = Color.green;
    }

    public void setPassive()
    {
        statusColor.color = new Color(0, 0, 0, 0);
    }
    
    
}
