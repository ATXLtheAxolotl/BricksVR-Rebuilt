using TMPro;
using UnityEngine;

public class RoomInfoTab : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomName;
    [SerializeField]
    private TextMeshProUGUI roomCode;
    [SerializeField]
    private TextMeshProUGUI roomOwner;
    void Awake()
    {
        roomName.text = NormalSessionManager.GetInstance().GetRoomName();
        roomCode.text = NormalSessionManager.GetInstance().GetRoomName();
        //roomOwner.text = NormalSessionManager.GetInstance();
    }
}
