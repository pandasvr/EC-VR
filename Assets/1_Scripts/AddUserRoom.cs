using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AddUserRoom : MonoBehaviour
{
    public GameObject prefab_ToggleUser;
    public GameObject scrollviewContent;

    private Dictionary<int, GameObject> togglesList;
    private float init_posY = -80;

    // Update is called once per frame
    public void GetAllUsers()
    {
        for (int i = 0; i < scrollviewContent.transform.childCount; i++)
        {
            Destroy(scrollviewContent.transform.GetChild(i).gameObject);
        }
        
        StartCoroutine(CoroutineGetAllUsers());
    }
    
    IEnumerator CoroutineGetAllUsers()
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
            var responseJson = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data, 3, www.downloadHandler.data.Length-3);
            Debug.Log("Json response :" + responseJson);

            if (!string.IsNullOrEmpty(responseJson))
            {
                float posY = 0;
                GameObject currentToggleUser;
                togglesList = new Dictionary<int, GameObject>();
                
                //On récupère l'id, le nom et le prénom de chaque utilisateur et on l'affiche
                for (int i = 0; i < JArray.Parse(responseJson).Count; i++)
                {
                    var r_item = JArray.Parse(responseJson)[i].ToString();
                    var r_idUser = JObject.Parse(r_item)["idUser"].ToString();
                    var r_userFirstName = JObject.Parse(r_item)["userFirstName"].ToString();
                    var r_userLastName = JObject.Parse(r_item)["userLastName"].ToString();
                    Debug.Log("Response for UserId n°" + r_idUser + " : " + r_userFirstName + " " + r_userLastName);
                    
                    //On vérifie si le userId correspond à l'Id de l'utilisateur connecté. Si oui, on n'ajoute pas son nom à la liste
                    if (int.Parse(r_idUser) != int.Parse(UnityEngine.PlayerPrefs.GetString("userId")))
                    {
                        posY += init_posY;
                        currentToggleUser = Instantiate(prefab_ToggleUser,scrollviewContent.transform);
                        currentToggleUser.GetComponent<RectTransform>().anchoredPosition = new Vector2(50,posY);
                        GameObject currentTextName = currentToggleUser.transform.Find("Label").gameObject;
                        currentTextName.GetComponent<Text>().text = r_userLastName + ", " + r_userFirstName;
                    
                        //on ajoute tout les toggles créés dans une liste
                        togglesList.Add(int.Parse(r_idUser), currentToggleUser);
                    }

                   
                }

            }

        }
    }

    public List<int> GetListInvites()
    {
        var listInvite = new List<int>();

        foreach (KeyValuePair<int, GameObject> toggle in togglesList)
        {
            if (toggle.Value.GetComponent<Toggle>().isOn == true)
            {
                listInvite.Add(toggle.Key);
            }
        }
        
        return listInvite;
    }
}
