using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    public InputField fieldName;
    public InputField fieldPassword;

    //Création d'une coroutine pour l'inscription d'un utilisateur
    public void Call()
    {
        StartCoroutine(ServerSend());
    }

    //Inscription de l'utilisateur
    IEnumerator ServerSend()
    {
        //Récupération des valeurs du formulaire
        WWWForm form = new WWWForm();
        form.AddField("name", fieldName.text);
        form.AddField("password", fieldPassword.text);
        
        Debug.Log("nameField :" + fieldName.text);
        Debug.Log("passwordField :" + fieldPassword.text);

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
            string responseText = www.downloadHandler.text;
            Debug.Log("Response Text:" + responseText);
        }
    }
}
