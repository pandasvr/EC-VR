using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    //champs noms et password récupérés depuis l'UI d'unity
    public InputField fieldName;
    public InputField fieldPassword;
    
    public GameObject LoginPanel;
    public GameObject MainPanel;
    public Text InfoText;

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
            var responseJson = www.downloadHandler.text;
            Debug.Log("Json response :" + responseJson);

            if (!string.IsNullOrEmpty(responseJson))
            {
                //Récupération des item JSON reçu depuis le serveur
                var r_userName = JObject.Parse(responseJson)["userName"].ToString();
                Debug.Log("Response username :" + r_userName);
                var r_userPassword = JObject.Parse(responseJson)["cryptPassword"].ToString();
                Debug.Log("Response password :" + r_userPassword);
                var r_userEmail = JObject.Parse(responseJson)["userEmail"].ToString();
                Debug.Log("Response user email :" + r_userEmail);
                var r_userLevel = JObject.Parse(responseJson)["userLevel"].ToString();
                Debug.Log("Response user level :" + r_userLevel);
                var r_labelUserLevel = JObject.Parse(responseJson)["labelUserLevel"].ToString();
                Debug.Log("Response label user level :" + r_userLevel);
                
                //création des playerPref servant de variables globales
                PlayerPrefs.SaveUser(r_userName, r_userEmail, r_userLevel, r_labelUserLevel);
                
                decryptedPassword = Crypting.Decrypt(r_userPassword);
                Debug.Log("Response Text decrypt :" + decryptedPassword);
                isValidPassword = Hashing.ValidatePassword(fieldPassword.text, decryptedPassword);

                if (isValidPassword)
                {
                    Debug.Log("user connected");
                    LoginPanel.SetActive(false);
                    MainPanel.SetActive(true);
                    fieldPassword.text=""; //on vide les champs pour que les identifiants ne soient pas "sauvegardés" par la page de connexion
                    fieldName.text = "";
                    
                    if (InfoText.enabled)
                    {
                        InfoText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("wrong username or password");
                    InfoText.text = "Nom d'utilisateur ou mot de passe incorrect";
                    InfoText.color = Color.red;
                    InfoText.enabled = true;
                    InfoText.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.Log("wrong username or password");
                InfoText.text = "Nom d'utilisateur ou mot de passe incorrect";
                InfoText.color = Color.red;
                InfoText.enabled = true;
                InfoText.gameObject.SetActive(true);
            }

        }
    }
}
