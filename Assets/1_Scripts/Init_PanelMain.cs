﻿using UnityEngine;

public class Init_PanelMain : MonoBehaviour
{
    public GameObject PanelLogin;
    public GameObject PanelMain;
    //public GameObject ButtonAdmin;
    
    void Update()
    {
        //Check user connected
        if (!PlayerPrefs.CheckUserConnected())
        {
            PanelLogin.SetActive(true);
            PanelMain.SetActive(false);
        }
        
        //Check Level administration
        if (UnityEngine.PlayerPrefs.GetString("userLevel") == "3")
        {
            //TODO Code d'apparition de la page admin à revoir
            //ButtonAdmin.SetActive(true);
        }
        else
        {
            //ButtonAdmin.SetActive(false);
        }
    }
}
