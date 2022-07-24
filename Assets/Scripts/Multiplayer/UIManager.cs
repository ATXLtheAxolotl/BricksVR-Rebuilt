using UnityEngine.UI;
using UnityEngine;
using RiptideNetworking;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    public static UIManager Singleton {
        get => _singleton;
        private set {
            if(_singleton == null) 
                _singleton = value;
            else if(_singleton != value) {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private GameObject sessionManager;

    private void Awake() 
    {
        Singleton = this;
    }

    public void ConnectClicked() 
    {
        connectUI.SetActive(false);

        NetworkManager.Singleton.Connect();
    }

    public void BackToMain() 
    {
        sessionManager.GetComponent<NormalSessionManager>().BackToMenuWrapper();
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString(UserSettings.GetInstance().Nickname);
        NetworkManager.Singleton.Client.Send(message);
    }
}
