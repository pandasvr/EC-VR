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

    public Text Text_Info;
    public GameObject Panel_Main;
    public GameObject Panel_CreateRoom;
    
    public InputField Field_RoomName;
    public Slider Slider_UserNumber;
    public Toggle Toggle_Whiteboard;
    public Toggle Toggle_PostIt;
    public Toggle Toggle_MediaProjection;
    public Toggle Toggle_ChatNonVr;
    public Dropdown Dropdown_Environnement;

    #endregion
    
    #region Private Fields

    string roomName;
    int userNumber;
    string whiteboard;
    string postIt;
    string mediaProjection;
    string chatNonVr;
    int environnement_id;

    #endregion

    public void Call_SaveRoom()
    {
        StartCoroutine(SaveRoom());
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
        
        switch (Slider_UserNumber.value)
        {
            case 1:
                userNumber = 5;
                break;
            case 2:
                userNumber = 10;
                break;
            case 3:
                userNumber = 15;
                break;
            default:
                userNumber = 5;
                break;
        }
        whiteboard = Toggle_Whiteboard.isOn.ToString();
        postIt = Toggle_PostIt.isOn.ToString();
        mediaProjection = Toggle_MediaProjection.isOn.ToString();
        chatNonVr = Toggle_ChatNonVr.isOn.ToString();
        environnement_id = Dropdown_Environnement.value + 1;
        
        //Create form values for send
        WWWForm form = new WWWForm();
        form.AddField("roomName", roomName);
        form.AddField("userNumber", userNumber);
        form.AddField("whiteboard", whiteboard);
        form.AddField("postIt", postIt);
        form.AddField("mediaProjection", mediaProjection);
        form.AddField("chatNonVr", chatNonVr);
        form.AddField("environnement_id", environnement_id);

        Debug.Log("roomName :" + roomName);
        Debug.Log("userNumber :" + userNumber);
        Debug.Log("whiteboard :" + whiteboard);
        Debug.Log("postIt :" + postIt);
        Debug.Log("mediaProjection :" + mediaProjection);
        Debug.Log("chatNonVr :" + chatNonVr);
        Debug.Log("environnement_id :" + environnement_id);

        //Envoie des données au serveur
        UnityWebRequest www = UnityWebRequest.Post(urlCreateRoom, form);

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
            bool responseText = BitConverter.ToBoolean(www.downloadHandler.data, 0);
            Debug.Log("Response Text:" + responseText);

            if (responseText)
            {
                Debug.Log("création réussie");
                Text_Info.text = "Création du salon réussie";
                Panel_Main.SetActive(true);
                Panel_CreateRoom.SetActive(false);
            }
            else
            {
                Debug.Log("création échouée");
                Text_Info.text = "Création du salon échouée";
            }
        }
    }
}
