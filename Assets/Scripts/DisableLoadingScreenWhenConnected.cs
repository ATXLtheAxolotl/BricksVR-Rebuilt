﻿using UnityEngine;

public class DisableLoadingScreenWhenConnected : MonoBehaviour
{
    public GameObject canvas;

    private void Start() {
        AvatarManager.GetInstance().AvatarCreated += HideLoadingScreen;

        canvas.SetActive(true);
    }

    private void HideLoadingScreen(PlayerAvatar avatar) {
        if(avatar.isLocal) canvas.SetActive(false);
    }
}
