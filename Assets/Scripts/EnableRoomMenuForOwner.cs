using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnableRoomMenuForOwner : MonoBehaviour
{
    public List<Button> buttons;
    public RoomOwnershipSync ownershipSync;
    public TextMeshProUGUI menuSubtitle;

    private const string NotOwnerText = "Only the room owner can change these settings.";

    void OnEnable()
    {
        try
        {
            if (gameObject.activeSelf)
            {
                bool isOwner = ownershipSync.IsRoomOwner();
                
                foreach(Button button in buttons) {
                    button.interactable = isOwner;
                }

                menuSubtitle.text = isOwner ? "" : NotOwnerText;
            }
        }
        catch (NullReferenceException)
        {
            Debug.LogError("Tried to check room ownership before we were connected to a room. Did you accidentally leave the room menu enabled while working on it @Nat >:)");
        }
    }
}
