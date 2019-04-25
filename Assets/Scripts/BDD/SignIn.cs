using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    //champs noms et password récupérés depuis l'UI d'unity
    public InputField fieldName;
    public InputField fieldPassword;

    private bool isValidPassword;
    private String userPassword;
    private String userName;
    private String decryptedPassword;

    //Création d'une coroutine pour la connexion d'un utilisateur
    public void Call()
    {
        StartCoroutine(ServerSend());
    }

    //Connexion de l'utilisateur
    IEnumerator ServerSend()
    {
        CspParameters cspParams = new CspParameters();

        // Les clés publique et privée
        RSAParameters privateKeys;
        RSAParameters publicKeys;

        using (var rsa = new RSACryptoServiceProvider(cspParams))
        {
            // Génère la clé publique et la clé privée
            privateKeys = rsa.ExportParameters(true);
            publicKeys = rsa.ExportParameters(false);

            rsa.Clear();
        }
        
        //Récupération des valeurs du formulaire
        userPassword = Hashing.HashPassword(fieldPassword.text); //on hache directement le mot de passe récupéré
        userName = fieldName.text;// on récupère directement le username
        
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
            Debug.Log("Response Text encrypt :" + responseText);
            
            decryptedPassword = Crypting.Decrypt(responseText);
            Debug.Log("Response Text decrypt :" + decryptedPassword);
            isValidPassword = Hashing.ValidatePassword(fieldPassword.text, decryptedPassword);
            
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
