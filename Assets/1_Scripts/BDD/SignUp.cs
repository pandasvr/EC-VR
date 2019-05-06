﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    //champs reliés à l'UI de l'application
    public InputField fieldName;
    public InputField fieldPassword;
    public InputField fieldEmail;
    
    public GameObject InscriptionPanel;
    public GameObject LoginPanel;
    public Text InfoTextLogin;
    public Text InfoTextInscription;

    //champs pour récupérer les informations entrées dans l'UI 
    private string userName;
    private string userEmail;
    private string userPassword;
    //champs dédiés à la sécurisation du mdp
    private string hashPassword;
    private string cryptPassword;

    
    //Création d'une coroutine pour l'inscription d'un utilisateur
    public void Call()
    {
        StartCoroutine(ServerSend());
    }

    //Cette fonction vérifie la validité des données entrées
    public bool ValidEntries(string theUserNameToCheck, string theUserEmailToCheck, string theUserPasswordToCheck)
    {
        
        return ( (theUserNameToCheck != "" && theUserPasswordToCheck != "" &&
                 theUserEmailToCheck != "") //aucun des champs renseignés n'est vide
                && (theUserPasswordToCheck.Length >= 4) //le mot de passe est au moins de longueur 4
                && (theUserEmailToCheck.Contains("@")) //un arobaz est présent dans le mail
            );
    }
    
    //Inscription de l'utilisateur
    IEnumerator ServerSend()
    {
        //Récupération des valeurs du formulaire
        userName = fieldName.text;
        userPassword = fieldPassword.text;
        userEmail = fieldEmail.text;
        
        
        //on ne peut pas accéder à l'étape suivante si les champs ne vérifient pas les conditions de la fonction ValidEntries
        if (ValidEntries(userName, userEmail, userPassword))
        {
            //vérification que le username choisi est unique:
            WWWForm formUserName = new WWWForm();
            formUserName.AddField("userName", userName);

            //Envoie des données au serveur
            UnityWebRequest wwwUserName = UnityWebRequest.Post("http://192.168.0.104/edsa-ecvr/conditionUniqueUserName.php", formUserName);
            
            //Récupération du retour serveur
            wwwUserName.downloadHandler = new DownloadHandlerBuffer();
            yield return wwwUserName.SendWebRequest();
            
            Debug.Log("reponse username :" + wwwUserName.downloadHandler.text);

            string reponseUserName = wwwUserName.downloadHandler.text;
            
            Debug.Log("reponse username :" + reponseUserName);
            
            //Si notre username est bien unique, les données sont envoyées au serveur:
            if (reponseUserName == "false")
            {
                Debug.Log(userName);

                //Hash password
                hashPassword = Hashing.HashPassword(userPassword);

                //Encrypt hashed password
                cryptPassword = Crypting.Encrypt(hashPassword);

                //Create form values for send
                WWWForm form = new WWWForm();
                form.AddField("userName", userName);
                form.AddField("cryptPassword", cryptPassword);
                form.AddField("userEmail", userEmail);

                Debug.Log("nameField :" + userName);
                Debug.Log("hashPassword :" + hashPassword);
                Debug.Log("cryptPassword :" + cryptPassword);
                Debug.Log("userEmail :" + userEmail);

                //Envoie des données au serveur
                UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1/edsa-unitySQL/signup.php", form);

                //Récupération du retour serveur
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                //Vérification du retour
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Post request complete!" + " Response Code: " + www.responseCode);
                    bool responseText = BitConverter.ToBoolean(www.downloadHandler.data, 0);
                    Debug.Log("Response Text:" + responseText);

                    if (responseText)
                    {
                        Debug.Log("Inscriptions réussie");
                        InscriptionPanel.SetActive(false);
                        LoginPanel.SetActive(true);
                        InfoTextLogin.text = "Inscription réussi !";
                        InfoTextLogin.color = Color.green;
                        InfoTextLogin.enabled = true;

                        if (InfoTextLogin.IsActive())
                        {
                            InfoTextInscription.enabled = false;
                        }
                    }
                    else
                    {
                        Debug.Log("Inscriptions échouée");
                        InfoTextInscription.text = "Une erreur est survenue";
                        InfoTextInscription.color = Color.red;
                        InfoTextInscription.enabled = true;
                    }
                }
            }
            //si le username est déjà pris:
            else
            {
                InfoTextInscription.text = "Ce nom d'utilisateur est déjà pris";
                InfoTextInscription.color = Color.red;
                InfoTextInscription.enabled = true;
                InfoTextInscription.gameObject.SetActive(true);
            }
        }
        
        //si un des champs est vide, on renvoie un message d'erreur
        else
        {
            Debug.Log("champs non valides");
            InfoTextInscription.text = "champs non valides";
            InfoTextInscription.color = Color.red;
            InfoTextInscription.enabled = true;
            InfoTextInscription.gameObject.SetActive(true);
        }
    }
}