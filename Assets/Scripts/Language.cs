using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Language : MonoBehaviour, IPointerClickHandler
{
    public string language;

    public void OnPointerClick(PointerEventData eventData)
    {
        I18n.Reinstantiate(language);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
