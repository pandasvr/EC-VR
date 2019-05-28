using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Init_PanelAddUserRoom : MonoBehaviour
{
    public GameObject ListObject_User;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GetAllUsers());
    }
    
    IEnumerator GetAllUsers()
    {
        //Création url envoie serveur
        string urlGetAllUsers = Adressing.GetAllUsersUrl();
        
        //Envoie des données au serveur
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(urlGetAllUsers, form);
        
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
                var r_idUser = JObject.Parse(responseJson)["idUser"].ToString();
                Debug.Log("Response idUser :" + r_idUser);
                var r_userName = JObject.Parse(responseJson)["userName"].ToString();
                Debug.Log("Response userName :" + r_userName);

            }
            else
            {
                
            }

        }
    }
}
