using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewJoin : MonoBehaviour
{
    // Start is called before the first frame update
    public void Connect() {
        RakClient.Connect("127.0.0.1", 8000);
        RakClient.OnInitialized += () => {
            Debug.Log("Initialized");
        };
    }
}
