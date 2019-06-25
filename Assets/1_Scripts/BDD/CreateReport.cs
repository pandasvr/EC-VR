using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreateReport : MonoBehaviour
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

    public AddUserRoom AddUserRoomScript;
    
    #endregion
    
    #region Private Fields

    string pathReport;
    private string dateReport;

    List<int> listinvites;
    private int createdRoomId;
    private int userCreator_id;
    
    private WWWForm form;
    private UnityWebRequest www;

    #endregion

    public void Call_SaveReport()
    {
        StartCoroutine(SaveReport());
    }

    IEnumerator SaveReport()
    {
        string urlCreateReport = Adressing.GetCreateReportUrl();

        //Create form values for send
        form = new WWWForm();
        form.AddField("pathReport", pathReport);
        form.AddField("dateReport", dateReport);

        Debug.Log("pathReport :" + pathReport);
        Debug.Log("dateReport :" + dateReport);


        //Envoie des données au serveur
        www = UnityWebRequest.Post(urlCreateReport, form);

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
            
            StartCoroutine(SaveReceivers());
        }
    }

    IEnumerator SaveReceivers()
    {
        string urlCreateReceiver = Adressing.GetCreateReceiverUrl();
        
        //Creation du formulaire d'invitation à la room pour le User Createur
        //Create form values for send
        form = new WWWForm();

        int idCreator = int.Parse(UnityEngine.PlayerPrefs.GetString("userId"));
        
        form.AddField("idUser", idCreator);
        form.AddField("idRoom", createdRoomId);
        
            
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
        Text_Info.text = "Création du salon réussie";
        Panel_Main.SetActive(true);
        Panel_CreateRoom.SetActive(false);
    }
}
