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

    public Friend friendInfo;

    public void Initialize(Friend data) {
        friendInfo = data;
        nameText.text = data.nickname;
        
        if(data.online && data.canVisit && data.location != null) {
            locationText.text = data.location;
            followButton.interactable = true;
        }
        else followButton.interactable = false;
    }

    public void FollowButtonPressed()
    {
        //BrickServerInterface.FollowFriend(friendCode);
        
    }
}
