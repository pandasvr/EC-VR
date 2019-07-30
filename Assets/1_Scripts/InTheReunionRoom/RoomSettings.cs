﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomDisconnect()
    {
        // plusieurs déconnexions simultanées ?? autre moyen de quitter la salle ??
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            StartCoroutine(CreateReport.SaveReport());
            // onclick button : imageProjecteur.sprite = Resources.Load <Sprite> ("MediaShare/Presentation1");
        }
        UnityEngine.PlayerPrefs.DeleteKey("idRoom");
        UnityEngine.PlayerPrefs.DeleteKey("whiteboard");
        UnityEngine.PlayerPrefs.DeleteKey("postIt");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }
}
