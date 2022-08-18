using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendListItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI locationText;
    public Button followButton;
    public string friendCode;

    private SyncVoiceWithSettings _syncVoiceWithSettings;

    public void FollowButtonPressed()
    {
        //BrickServerInterface.GetFriendLocation(friendCode);
        
    }
}
