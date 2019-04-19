using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    public InputField fieldName;
    public InputField fieldPassword;

    private String userName;
    private String userPassword;
    private String hashPassword;
    private String cryptPassword;

    private String tempEmail = "test@test.com";

    //Création d'une coroutine pour l'inscription d'un utilisateur
    public void Call()
    {
        StartCoroutine(ServerSend());
    }

    //Inscription de l'utilisateur
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
        userName = fieldName.text;
        userPassword = fieldPassword.text;

        //Hash password
        hashPassword = Hashing.HashPassword(userPassword);

        //Encrypt hashed password
        cryptPassword = Convert.ToBase64String(Crypting.Encrypt(hashPassword, publicKeys));
        
        //Create form values for send
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("hashPassword", cryptPassword);
        form.AddField("userEmail", tempEmail);
        
        Debug.Log("nameField :" + userName);
        Debug.Log("hashPassword :" + hashPassword);
        Debug.Log("cryptPassword :" + cryptPassword);
        Debug.Log("userEmail :" + tempEmail);

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
