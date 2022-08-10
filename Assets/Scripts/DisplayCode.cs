using TMPro;
using UnityEngine;

public class DisplayCode : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetCode(string code) {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = $"Room Code: {code}";
    }
}
