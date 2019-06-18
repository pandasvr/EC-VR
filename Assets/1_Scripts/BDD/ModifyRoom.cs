using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using _1_Scripts.Class;
using Newtonsoft.Json.Linq;
using Random = UnityEngine.Random;

public class ModifyRoom : MonoBehaviour
{
    #region Public Fields

    public Toggle Toggle_Whiteboard;
    public Toggle Toggle_PostIt;
    public Toggle Toggle_MediaProjection;
    public Toggle Toggle_ChatNonVr;
    public Dropdown Dropdown_Environnement;

    #endregion
    
    #region Private Fields

    string roomName;
    string whiteboard;
    string postIt;
    string mediaProjection;
    string chatNonVr;
    int environnement_id;
    
    List<int> listinvites;
    private int createdRoomId;

    private Room actualRoom;

    private Dictionary<int, GameObject> togglesList;
    private float init_posY = 80;

    #endregion

    public void Call_GetActualRoom()
    {
        StartCoroutine(GetActualRoom());
    }

    public void Call_SaveRoom()
    {
        StartCoroutine(SaveRoom());
    }

    IEnumerator GetActualRoom()
    {
        //Récupération du nom de la room
        roomName = PhotonNetwork.CurrentRoom.Name;
        Debug.Log(roomName);
        
        //Création url envoie serveur
        string urlGetRoom = Adressing.GetModifyRoomUrl_getRoom();

        //Création du formulaire de donnée
        WWWForm formGetRoom = new WWWForm();
        formGetRoom.AddField("roomName", roomName);

        //Envoie des données au serveur
        UnityWebRequest wwwGetRoom = UnityWebRequest.Post(urlGetRoom, formGetRoom);

        //Récupération du retour serveur
        wwwGetRoom.downloadHandler = new DownloadHandlerBuffer();
        yield return wwwGetRoom.SendWebRequest();

        
        //Vérification du retour
        if (wwwGetRoom.isNetworkError || wwwGetRoom.isHttpError)
        {
            Debug.Log(wwwGetRoom.error);
        }
        else
        {
            Debug.Log("Post request complete!" + " Response Code: " + wwwGetRoom.responseCode);
            var responseJsonGetRoom = System.Text.Encoding.UTF8.GetString(wwwGetRoom.downloadHandler.data,
                3,
                wwwGetRoom.downloadHandler.data.Length - 3);
            Debug.Log("Json response :" + responseJsonGetRoom);

            if (responseJsonGetRoom != "")
            {
                actualRoom = JsonConvert.DeserializeObject<Room>(responseJsonGetRoom);

                if (actualRoom.whiteboard == "true")
                {
                    Toggle_Whiteboard.isOn = true;
                }
                
                if (actualRoom.chatNonVr == "true")
                {
                    Toggle_ChatNonVr.isOn = true;
                }
                
                if (actualRoom.mediaProjection == "true")
                {
                    Toggle_MediaProjection.isOn = true;
                }
                
                if (actualRoom.postIt == "true")
                {
                    Toggle_PostIt.isOn = true;
                }

                Dropdown_Environnement.value = Int32.Parse(actualRoom.environnement_id);
                
                Debug.Log(actualRoom.ToString());
            }
        }
    }
    

    IEnumerator SaveRoom()
    {
        string urlModifyRoom = Adressing.GetModifyRoomUrl();

        roomName = PhotonNetwork.CurrentRoom.Name;
        whiteboard = Toggle_Whiteboard.isOn.ToString();
        postIt = Toggle_PostIt.isOn.ToString();
        mediaProjection = Toggle_MediaProjection.isOn.ToString();
        chatNonVr = Toggle_ChatNonVr.isOn.ToString();
        environnement_id = Dropdown_Environnement.value + 1;
        
        WWWForm formModifyRoom = new WWWForm();
        
        //Create form values for send
        formModifyRoom = new WWWForm();
        formModifyRoom.AddField("idRoom", actualRoom.idRoom);
        formModifyRoom.AddField("whiteboard", whiteboard);
        formModifyRoom.AddField("postIt", postIt);
        formModifyRoom.AddField("mediaProjection", mediaProjection);
        formModifyRoom.AddField("chatNonVr", chatNonVr);
        formModifyRoom.AddField("environnement_id", environnement_id);

        Debug.Log("idRoom :" + actualRoom.idRoom); 
        Debug.Log("whiteboard :" + whiteboard);
        Debug.Log("postIt :" + postIt);
        Debug.Log("mediaProjection :" + mediaProjection);
        Debug.Log("chatNonVr :" + chatNonVr);
        Debug.Log("environnement_id :" + environnement_id);

        //Envoi des données au serveur
        UnityWebRequest wwwModifyRoom = UnityWebRequest.Post(urlModifyRoom, formModifyRoom);

        //Récupération du retour serveur
        wwwModifyRoom.downloadHandler = new DownloadHandlerBuffer();
        yield return wwwModifyRoom.SendWebRequest();

        //Vérification du retour
        if (wwwModifyRoom.isNetworkError || wwwModifyRoom.isHttpError)
        {
            Debug.Log(wwwModifyRoom.error);
        }
        else
        {
            Debug.Log("Post request complete!" + " Response Code: " + wwwModifyRoom.responseCode);

            var responseData = int.Parse(System.Text.Encoding.UTF8.GetString(wwwModifyRoom.downloadHandler.data, 3, wwwModifyRoom.downloadHandler.data.Length-3)) ;
            Debug.Log("Response text last Id : " + responseData);

            createdRoomId = responseData;
            
            //StartCoroutine(SaveInvites());
        }
    }
}
