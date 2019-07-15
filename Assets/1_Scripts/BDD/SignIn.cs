﻿using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    //champs noms et password récupérés depuis l'UI d'unity
    public InputField fieldName;
    public InputField fieldPassword;
    public Animator Animator_Canva_Main;
    
    //public GameObject LoginPanel;
    //public GameObject MainPanel;

    private bool isValidPassword;
    private String userPassword;
    private String userName;
    private string userFirstname;
    private String userLastName;
    private String decryptedPassword;

    //Création d'une coroutine pour la connexion d'un utilisateur
    public void Call()
    {
        StartCoroutine(ServerSend());
    }

    //Connexion de l'utilisateur
    IEnumerator ServerSend()
    {
        //Création url envoie serveur
        string urlSignin = Adressing.GetSignInUrl();
        
        //Récupération des valeurs du formulaire
        userPassword = Hashing.HashPassword(fieldPassword.text); //on hache directement le mot de passe récupéré
        userName = fieldName.text;// on récupère directement le username
        
        //Création du formulaire de donnée
        WWWForm form = new WWWForm();
        form.AddField("name", userName);
        
        //Envoie des données au serveur
        UnityWebRequest www = UnityWebRequest.Post(urlSignin, form);
        
        //Récupération du retour serveur
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
 
        
        //Vérification du retour
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Post request complete!" + " Response Code: " + www.responseCode);
            var responseJson = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data, 3, www.downloadHandler.data.Length-3);

            Debug.Log("Json response :" + responseJson);

            if (responseJson != "")
            {
              var user = JsonConvert.DeserializeObject<User>(responseJson);

                //création des playerPref servant de variables globales
                PlayerPrefs.SaveUser(user.idUser, user.userName, user.userEmail, user.userLevel,  user.labelUserLevel, user.userFirstName, user.userLastName);

                
                decryptedPassword = Crypting.Decrypt(user.cryptPassword);
                Debug.Log("Response Text decrypt :" + decryptedPassword);
                isValidPassword = Hashing.ValidatePassword(fieldPassword.text, decryptedPassword);

                if (isValidPassword)
                {
                    Debug.Log("user connected");
                    //LoginPanel.SetActive(false);
                    //MainPanel.SetActive(true);
                    fieldPassword.text=""; //on vide les champs pour que les identifiants ne soient pas "sauvegardés" par la page de connexion
                    fieldName.text = "";
                    Animator_Canva_Main.Play("Fade_Login");
                }
                else
                {
                    Debug.Log("wrong username or password");
                }
            }
            else
            {
                Debug.Log("wrong username or password");
            }

        }
    }
}
