using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Export : MonoBehaviour
{

    private GameObject whiteboard;
    private GameObject paintingReceiver;
    
    static WWWForm form; 
    static UnityWebRequest www;
    private static string idRoom;
    
    public void ExportPNG()
    {
        whiteboard = GameObject.FindGameObjectsWithTag("Whiteboard")[0];
        paintingReceiver = whiteboard.transform.Find("PaintingReceiver").gameObject;

        var tex = paintingReceiver.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        
        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();

        var date = DateTime.Today;

        var pathName = "Resources/Documents/Capture/" + date.ToString("yyyy-MM-ddTHH-mm-ss") + ".png";
        
        File.WriteAllBytes( pathName, bytes);

        StartCoroutine(SaveReport(pathName, date.ToString()));
        
        Debug.Log("Capture enregistrée");

    }
    
    public void ExportPPT()
    {
        var date = DateTime.Today;

        var pathName = "Resources/Documents/Capture/Whiteboard.pptx";

        StartCoroutine(SaveReport(pathName, date.ToString()));
        
        Debug.Log("PPT enregistrée");
    }

    public void ExportExcel()
    {
        var date = DateTime.Today;

        var pathName = "Resources/Documents/Capture/TodoList.xlsx";

        StartCoroutine(SaveReport(pathName, date.ToString()));
        
        Debug.Log("Excel enregistrée");
    }

    public static IEnumerator SaveReport(string pathName, string date)
    {
        string urlCreateReport = Adressing.GetCreateReportUrl();

        //Create form values for send
        form = new WWWForm();
        idRoom = UnityEngine.PlayerPrefs.GetString("idRoom");
        
        form.AddField("pathReport", pathName);
        form.AddField("dateReport", date);
        form.AddField("idRoom", idRoom);

        Debug.Log("pathReport :" + pathName);
        Debug.Log("dateReport :" + date);
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
