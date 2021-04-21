using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreScript : MonoBehaviour
{

    public Text positionText;
    public Text nameText;
    public Text scoreText;

    public void Init(int position, string name, int score)
    {
        positionText.text = "#" + position;
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
