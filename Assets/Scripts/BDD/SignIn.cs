﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    public InputField fieldName;
    public InputField fieldPassword;

    private bool isValidPassword;
    private String userPassword;
    private String userName;

    //Création d'une coroutine pour la connexion d'un utilisateur
    public void Call()
    {
        StartCoroutine(ServerSend());
    }

    //Connexion de l'utilisateur
    IEnumerator ServerSend()
    {
        //Récupération des valeurs du formulaire
        userPassword = Hashing.HashPassword(fieldPassword.text);
        userName = fieldName.text;
        
        WWWForm form = new WWWForm();
        form.AddField("name", userName);
        
        Debug.Log("nameField :" + userName);
        Debug.Log("passwordField :" + userPassword);

        //Envoie des données au serveur
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1/edsa-unitySQL/signin.php", form);
        
        //Récupération du retour serveur
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
 
        //Vérification du retour
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Post request complete!" + " Response Code: " + www.responseCode);
            string responseText = www.downloadHandler.text;
            Debug.Log("Response Text:" + responseText);
            isValidPassword = Hashing.ValidatePassword(fieldPassword.text, responseText);
            
            if (isValidPassword)
            {
                Debug.Log("user connected");
            }
            else
            {
                Debug.Log("wrong name or password");
            }
            
        }
    }
}
