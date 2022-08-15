using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI displayTitle;
    [SerializeField]
    public TextMeshProUGUI displayMessage;
    [SerializeField]
    private GameObject displayButton;

    

    public void DisplayMessage(string title, string message, bool closable = false) {
        displayTitle.text = title;
        displayMessage.text = message;
        if(closable) displayButton.SetActive(true);
        else displayButton.SetActive(false);

        this.gameObject.SetActive(true);
    }
}
