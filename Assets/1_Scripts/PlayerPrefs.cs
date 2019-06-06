using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using Vector3 = UnityEngine.Vector3;

public class PlayerPrefs : MonoBehaviour
{
    public static void SaveUser(string userId, string userName, string userEmail, string userLevel, string labelUserLevel)
    {
    }

    public static void SaveUser(string userId, string userName, string userEmail, string userLevel, string labelUserLevel, string userFirstName, string userLastName)
    {
        UnityEngine.PlayerPrefs.SetString("userId", userId);
        UnityEngine.PlayerPrefs.SetString("userName", userName);
        UnityEngine.PlayerPrefs.SetString("userEmail", userEmail);
        UnityEngine.PlayerPrefs.SetString("userLevel", userLevel);
        UnityEngine.PlayerPrefs.SetString("labelUserLevel", labelUserLevel);
        UnityEngine.PlayerPrefs.SetString("userFirstName", userFirstName);
        UnityEngine.PlayerPrefs.SetString("userLastName", userLastName);
        
        UnityEngine.PlayerPrefs.SetString("status", "connecté");
    }

    public void Disconnect()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
        Debug.Log("User disconnected");
    }

    public static bool CheckUserConnected()
    {
        bool rt;
        
        if (UnityEngine.PlayerPrefs.HasKey("userName"))
        {
            rt = true;
        }
        else
        {
            rt = false;
        }

        return rt;
    }

    

    protected bool userMoves(Vector3 PreviousPosition, Vector3 Newposition)
    {
        bool userIsMoving = true;
        if (false)
        {
            userIsMoving = false;
        }
        return userIsMoving;
    }
    
    //fonction qui change la valeur du statut de l'utilisateur
    protected string updateStatus(float timer, Vector3 previousPosition, Vector3 newPosition)
    {
        //le string auquel nous allons attribuer unee valeur selon certaines conditions
        string status;
        
        if (userMoves(PreviousPosition, Newposition))
        {
            //if player in menu if player not in menu
            timer = 0;
            status = "connecté";
        }
        
        //Tout d'abord, la condition pour savoir si l'utilisateur est absent
        //condition sur un timer
        if (timer == 120)
        {
            status = "absent";
        }

        //Ensuite, condition pour savoir si l'utilisateur est dans son menu personnel
        if (false)
        {
            status = "Dans le Menu";
        }
        
        //Sinon, l'utilisateur est présent dans le salon, donc connecté
        else
        {
            status = "connecté";
        }
        Debug.Log(status);
        return status;
    }
    
    
    
    protected Vector3 PreviousPosition;
    protected Vector3 Newposition;
    protected float timer;
    protected string status;
    private void Start()
    {
        //previousposition=
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
     
        //à chaque frame, on regarde si le statut a changé
        updateStatus(timer, PreviousPosition,Newposition);
    }
}
