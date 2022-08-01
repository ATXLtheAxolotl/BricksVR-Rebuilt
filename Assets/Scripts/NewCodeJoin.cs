using UnityEngine;
using TMPro;

public class NewCodeJoin : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI codeText;
    [SerializeField]
    private NormalSessionManager manager;
    [SerializeField]
    public TextMeshProUGUI displayTitle;
    [SerializeField]
    public TextMeshProUGUI displayMessage;
    [SerializeField]
    private GameObject displayButton;

    private string code = "";

    public void Press(int number) {
        if(code.Length > 7) 
            return;

        code += number;
        codeText.text = $"CODE:\n{code}";
    }

    public void Delete() {
        if (code.Length <= 0) {
            Debug.Log("Cancelling not long enough");
            return;
        }

        code = code.Remove(code.Length - 1, 1);
        codeText.text = $"CODE:\n{code}";
    }

    public void DisplayMessage(string title, string message, bool closable = false) {
        displayTitle.text = title;
        displayMessage.text = message;
        if(closable) displayButton.SetActive(true);
        else displayButton.SetActive(false);

        displayTitle.transform.parent.gameObject.SetActive(true);
    }
    public void Go() {
        Debug.Log($"Joining {code}");
        manager.JoinRoomWrapper(code);
    }
}
