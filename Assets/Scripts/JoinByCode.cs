using TMPro;
using UnityEngine;

public class JoinByCode : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI codeText;
    [SerializeField]
    private NormalSessionManager manager;

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

    public void Go() {
        Debug.Log($"Joining {code}");
        manager.JoinRoomWrapper(code);
    }
}
