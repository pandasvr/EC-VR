using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    public InputField fieldName;
    public InputField fieldPassword;

    private String userName;
    private String userPassword;

    //Création d'une coroutine pour l'inscription d'un utilisateur
    public void Call()
    {
        StartCoroutine(ServerSend());
    }

    //Inscription de l'utilisateur
    IEnumerator ServerSend()
    {
        //Récupération des valeurs du formulaire
        userName = fieldName.text;
        userPassword = Hashing.HashPassword(fieldPassword.text);
        
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("userPassword", userPassword);
        
        Debug.Log("nameField :" + userName);
        Debug.Log("passwordField :" + userPassword);

        //Envoie des données au serveur
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1/edsa-unitySQL/signup.php", form);
        
        //Récupération du retour serveur
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
 
        //Vérification du retour
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Post request complete!" + " Response Code: " + www.responseCode);
            bool responseText = BitConverter.ToBoolean(www.downloadHandler.data, 0);
            Debug.Log("Response Text:" + responseText);
        }
    }
}
