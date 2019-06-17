using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using _1_Scripts.Class;
using Random = UnityEngine.Random;

public class ModifyRoom : MonoBehaviour
{
    #region Public Fields

    public Toggle Toggle_Whiteboard;
    public Toggle Toggle_PostIt;
    public Toggle Toggle_MediaProjection;
    public Toggle Toggle_ChatNonVr;
    public Dropdown Dropdown_Environnement;

    public AddUserRoom AddUserRoomScript;
    
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

    private WWWForm form;
    private UnityWebRequest www;

    #endregion

    public void Call_GetActualRoom()
    {
        StartCoroutine(GetActualRoom());
    }

    public void Call_SaveRoom()
    {
        listinvites = AddUserRoomScript.GetListInvites();
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
        
        //Create form values for send
        form = new WWWForm();
        form.AddField("idRoom", actualRoom.idRoom);
        form.AddField("whiteboard", whiteboard);
        form.AddField("postIt", postIt);
        form.AddField("mediaProjection", mediaProjection);
        form.AddField("chatNonVr", chatNonVr);
        form.AddField("environnement_id", environnement_id);

        Debug.Log("idRoom :" + actualRoom.idRoom); 
        Debug.Log("whiteboard :" + whiteboard);
        Debug.Log("postIt :" + postIt);
        Debug.Log("mediaProjection :" + mediaProjection);
        Debug.Log("chatNonVr :" + chatNonVr);
        Debug.Log("environnement_id :" + environnement_id);

        //Envoi des données au serveur
        www = UnityWebRequest.Post(urlModifyRoom, form);

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

            var responseData = int.Parse(System.Text.Encoding.UTF8.GetString(www.downloadHandler.data, 3, www.downloadHandler.data.Length-3)) ;
            Debug.Log("Response text last Id : " + responseData);

            createdRoomId = responseData;
            
            StartCoroutine(SaveInvites());
        }
    }

    IEnumerator SaveInvites()
    {
        string urlCreateInvite = Adressing.GetCreateInviteUrl();
        
        //Creation du formulaire d'invitation à la room pour le User Createur
        //Create form values for send
        form = new WWWForm();

        int idCreator = int.Parse(UnityEngine.PlayerPrefs.GetString("userId"));
        
        form.AddField("idUser", idCreator);
        form.AddField("idRoom", createdRoomId);
        form.AddField("isCreator",1);
            
        //Envoie des données au serveur
        www = UnityWebRequest.Post(urlCreateInvite, form);

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
            bool textResponse = BitConverter.ToBoolean(www.downloadHandler.data, 0);
            if (textResponse)
            {
                Debug.Log("invitation user n°" + idCreator + " à la room n°"+ createdRoomId + " réussie");
            }
            else
            {
                Debug.Log("invitation user n°" + idCreator + " à la room n°"+ createdRoomId + " échouée");
            }
        }
        
        foreach (int userId in listinvites)
        {
            //Create form values for send
            form = new WWWForm();
            form.AddField("idUser", userId);
            form.AddField("idRoom", createdRoomId);
            form.AddField("isCreator",0);
            
            //Envoie des données au serveur
            www = UnityWebRequest.Post(urlCreateInvite, form);

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
                bool textResponse = BitConverter.ToBoolean(www.downloadHandler.data, 0);
                if (textResponse)
                {
                    Debug.Log("invitation user n°" + userId + " à la room n°"+ createdRoomId + " réussie");
                }
                else
                {
                    Debug.Log("invitation user n°" + userId + " à la room n°"+ createdRoomId + " échouée");
                }
            }
        }
        
        Debug.Log("modification réussie");
    }
}
