using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreateRoom : MonoBehaviour
{
    #region Public Fields
    
    [Header("Options room")]
    public InputField Field_RoomName;
    public Dropdown Dropdown_Environnement;

    [Header("Toggles option")] 
    public Toggle Toggle_maxUser_1_5;
    public Toggle Toggle_maxUser_6_10;
    public Toggle Toggle_maxUser_11_15;
    public Toggle Toggle_Whiteboard;
    public Toggle Toggle_PostIt;
    public Toggle Toggle_MediaProjection;
    public Toggle Toggle_ChatNonVr;
    
    [Header("Script AddUserRoom")]
    public AddUserRoom AddUserRoomScript;
    
    #endregion
    
    #region Private Fields

    string roomName;
    int userNumber;
    string whiteboard;
    string postIt;
    string mediaProjection;
    string chatNonVr;
    int environnement_id;
    List<int> listinvites;
    private int createdRoomId;
    private int userCreator_id;
    
    private WWWForm form;
    private UnityWebRequest www;

    #endregion

    public void Call_SaveRoom()
    {
        listinvites = AddUserRoomScript.GetListInvites();
        if (listinvites.Count == 0 )
        {
            Debug.Log("Erreur, pas d'utilisateurs invités");
        }
        else
        {
            StartCoroutine(SaveRoom());
        }
    }

    IEnumerator SaveRoom()
    {
        string urlCreateRoom = Adressing.GetCreateRoomUrl();
        
        //Récupération des valeurs du formulaire
        if (Field_RoomName.text != "")
        {
            roomName = Field_RoomName.text;
        }
        else
        {
            roomName = "Salon#" + Random.Range(1, 9999);
        }

        if (Toggle_maxUser_1_5.isOn)
        {
            userNumber = 5;
        }
        else if (Toggle_maxUser_6_10.isOn)
        {
            userNumber = 10;
        }
        else
        {
            userNumber = 15;
        }
        
        whiteboard = Toggle_Whiteboard.isOn.ToString();
        postIt = Toggle_PostIt.isOn.ToString();
        mediaProjection = Toggle_MediaProjection.isOn.ToString();
        chatNonVr = Toggle_ChatNonVr.isOn.ToString();
        
        
        environnement_id = Dropdown_Environnement.value + 1;
        userCreator_id = int.Parse(UnityEngine.PlayerPrefs.GetString("userId"));
        
        //Create form values for send
        form = new WWWForm();
        form.AddField("roomName", roomName);
        form.AddField("userNumber", userNumber);
        form.AddField("whiteboard", whiteboard);
        form.AddField("postIt", postIt);
        form.AddField("mediaProjection", mediaProjection);
        form.AddField("chatNonVr", chatNonVr);
        form.AddField("environnement_id", environnement_id);
        form.AddField("userCreator_id", userCreator_id);

        Debug.Log("roomName :" + roomName);
        Debug.Log("userNumber :" + userNumber);
        Debug.Log("whiteboard :" + whiteboard);
        Debug.Log("postIt :" + postIt);
        Debug.Log("mediaProjection :" + mediaProjection);
        Debug.Log("chatNonVr :" + chatNonVr);
        Debug.Log("environnement_id :" + environnement_id);
        Debug.Log("userCreator_Id :" + userCreator_id);

        //Envoie des données au serveur
        www = UnityWebRequest.Post(urlCreateRoom, form);

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
        
        Debug.Log("création réussie");
    }
}
