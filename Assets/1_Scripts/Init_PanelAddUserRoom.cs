using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Init_PanelAddUserRoom : MonoBehaviour
{
    public GameObject ListObject_User;

    // Update is called once per frame
    public void GetAllUsersBis()
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
                for (int i = 0; i < JArray.Parse(responseJson).Count; i++)
                {
                    var r_item = JArray.Parse(responseJson)[i].ToString();
                    var r_idUser = JObject.Parse(r_item)["idUser"].ToString();
                    var r_userFirstName = JObject.Parse(r_item)["userFirstName"].ToString();
                    var r_userLastName = JObject.Parse(r_item)["userLastName"].ToString();
                    Debug.Log("Response for UserId n°" + r_idUser + " : " + r_userFirstName + " " + r_userLastName);
                }

            }
            else
            {
                
            }

        }
    }
}
