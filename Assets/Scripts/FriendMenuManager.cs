using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendMenuManager : MonoBehaviour
{
    public GameObject friendEntryPrefab;

    public string[] friendCodes = UserSettings.GetInstance().FriendCodes.Split(char.Parse(";"));

    public GameObject listParent;
    private Transform _listParentTransform;

    // Start is called before the first frame update
    void Awake()
    {
        _listParentTransform = listParent.transform;
    }

    private void OnEnable()
    {
        RefreshPlayerList();
    }

    private void RefreshPlayerList()
    {
        RebuildUI();
        foreach(string code in friendCodes) {
            Instantiate(friendEntryPrefab, _listParentTransform);
        }
    }

    public IEnumerator<Coroutine> RebuildUI()
    {
        CoroutineWithData friends =
            new CoroutineWithData(this, BrickServerInterface.GetInstance().GetFriendsIEnum());
        yield return friends.coroutine;

        foreach (Transform t in _listParentTransform)
            Destroy(t.gameObject);

        foreach (Friend data in friends.result as Friend[])
        {
            GameObject newPlayerEntry = Instantiate(friendEntryPrefab, _listParentTransform);
            FriendListItem friendListItem = newPlayerEntry.GetComponent<FriendListItem>();
            friendListItem.Initialize(data);
        }
    }
}