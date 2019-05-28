using System;
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
    public InputField fieldFirstName;
    public InputField fieldLastName;
    public Dropdown fieldUserLevel;
    
    public GameObject InscriptionPanel;
    public GameObject LoginPanel;
    
    public Text InfoTextInscription;

    //champs pour récupérer les informations entrées dans l'UI 
    private string userName;
    private string userEmail;
    private string userPassword;
    private int userLevel;
    
    private string userFirstName;
    private string userLastName;
    
    //champs dédiés à la sécurisation du mdp
    private string hashPassword;
    private string cryptPassword;

    
    //Création d'une coroutine pour l'inscription d'un utilisateur
    public void Call()
    {
        StartCoroutine(ServerSend());
    }

    //Cette fonction vérifie la validité des données entrées
    public bool ValidEntries(string theUserNameToCheck, string theUserEmailToCheck, string theUserPasswordToCheck, string theUserFirstNameToCheck, string theUserLastNameToCheck)
    {
        return ( (theUserNameToCheck != "" && theUserPasswordToCheck != "" && theUserEmailToCheck != "" && theUserFirstNameToCheck != "" && theUserLastNameToCheck != "") //aucun des champs renseignés n'est vide
                && (theUserPasswordToCheck.Length >= 4) //le mot de passe est au moins de longueur 4
                && (theUserEmailToCheck.Contains("@") //un arobaz est présent dans le mail
                && fieldUserLevel.value != 0) //Le champ "role" est renseigné
            );
    }
    
    //Inscription de l'utilisateur
    IEnumerator ServerSend()
    {
        //Création des url d'envoie au seveur
        string urlConditionUniqueUserName = Adressing.GetSignUpUrl_ConditionUniqueUserName();
        string urlSignup = Adressing.GetSignUpUrl();
        
        //Récupération des valeurs du formulaire
        userName = fieldName.text;
        userPassword = fieldPassword.text;
        userEmail = fieldEmail.text;
        userLevel = fieldUserLevel.value;
        userFirstName = fieldFirstName.text;
        userLastName = fieldLastName.text;
        
        
        //on ne peut pas accéder à l'étape suivante si les champs ne vérifient pas les conditions de la fonction ValidEntries
        if (ValidEntries(userName, userEmail, userPassword, userFirstName, userLastName))
        {
            //vérification que le username choisi est unique:
            WWWForm formUserName = new WWWForm();
            formUserName.AddField("userName", userName);

            //Envoie des données au serveur
            UnityWebRequest wwwUserName = UnityWebRequest.Post(urlConditionUniqueUserName, formUserName);
            
            //Récupération du retour serveur
            wwwUserName.downloadHandler = new DownloadHandlerBuffer();
            yield return wwwUserName.SendWebRequest();
            
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
                form.AddField("userLevel", userLevel);
                form.AddField("userFirstName", userFirstName);
                form.AddField("userLastName", userLastName);

                Debug.Log("nameField :" + userName);
                Debug.Log("hashPassword :" + hashPassword);
                Debug.Log("cryptPassword :" + cryptPassword);
                Debug.Log("userEmail :" + userEmail);
                Debug.Log("userFirstName :" + userFirstName);
                Debug.Log("userLastName :" + userLastName);
                Debug.Log("userLevel :" + userEmail);

                //Envoie des données au serveur
                UnityWebRequest www = UnityWebRequest.Post(urlSignup, form);

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
                        Debug.Log("Inscription réussie");
                        InfoTextInscription.text = "Inscription réussie !";
                        InfoTextInscription.color = Color.green;
                        InfoTextInscription.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("Inscriptions échouée");
                        InfoTextInscription.text = "Une erreur est survenue";
                        InfoTextInscription.color = Color.red;
                        InfoTextInscription.gameObject.SetActive(true);
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
    
    public void ViderFormulaire()
    {
        InfoTextInscription.gameObject.SetActive(false);
        fieldName.text = "";
        fieldPassword.text = "";
        fieldEmail.text = "";
    }
}
