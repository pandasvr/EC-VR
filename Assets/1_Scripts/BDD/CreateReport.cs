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

    public AddUserRoom AddUserRoomScript;
    
    #endregion
    
    #region Private Fields

    string pathReport;
    private string dateReport;

    List<int> listreceivers;
    private int createdReportId;
    //private int userCreator_id;
    
    private WWWForm form;
    private UnityWebRequest www;

    #endregion

    public void Call_SaveReport()
    {
        listreceivers = AddUserRoomScript.GetListInvites();
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

            createdReportId = responseData;
            
            StartCoroutine(SaveReceivers());
        }
    }

    IEnumerator SaveReceivers()
    {
        string urlCreateReceiver = Adressing.GetCreateReceiverUrl();
        
        /*//Creation du formulaire d'invitation à la room pour le User Createur
        //Create form values for send
        form = new WWWForm();

        int idReport = int.Parse(UnityEngine.PlayerPrefs.GetString("userId"));
        
        form.AddField("idUser", idReport);
        form.AddField("idRoom", createdRoomId);
        
            
        //Envoie des données au serveur
        www = UnityWebRequest.Post(urlCreateReceiver, form);

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
        }*/
        
        foreach (int userId in listreceivers)
        {
            //Create form values for send
            form = new WWWForm();
            form.AddField("idUser", userId);
            form.AddField("idReport", createdReportId);

            //Envoie des données au serveur
            www = UnityWebRequest.Post(urlCreateReceiver, form);

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
                    Debug.Log("invitation user n°" + userId + " au report n°"+ createdReportId + " réussie");
                }
                else
                {
                    Debug.Log("invitation user n°" + userId + " au report n°"+ createdReportId + " échouée");
                }
            }
        }
        
        Debug.Log("création réussie");
        Text_Info.text = "Création du compte rendu réussie";
        
    }
}
