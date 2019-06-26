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

    public static AddUserRoom AddUserRoomScript;

    #endregion

    #region Private Fields

    static string pathReport;
    static string dateReport;

    static List<int> listreceivers;
    static int createdReportId;
    //private int userCreator_id;

    static WWWForm form; 
    static UnityWebRequest www;
    private static string idRoom;

    #endregion

    public void Call_SaveReport()
    {
        StartCoroutine(SaveReport());
    }

    public static IEnumerator SaveReport()
    {
        string urlCreateReport = Adressing.GetCreateReportUrl();

        //Create form values for send
        form = new WWWForm();
        pathReport = "Assets/Resources/Documents/CR/CompteRendu.pdf";
        dateReport = DateTime.Now.ToString();
        idRoom = UnityEngine.PlayerPrefs.GetString("idRoom");
        
        form.AddField("pathReport", pathReport);
        form.AddField("dateReport", dateReport);
        form.AddField("idRoom", idRoom);

        Debug.Log("pathReport :" + pathReport);
        Debug.Log("dateReport :" + dateReport);
        Debug.Log("idRoom :" + idRoom);

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
            
            bool responseText = BitConverter.ToBoolean(www.downloadHandler.data, 0);
            Debug.Log("Response Text:" + responseText);

            if (responseText)
            {
                Debug.Log("création réussie");
            }
            else
            {
                Debug.Log("création échouée");
            }

        }
    }

}
