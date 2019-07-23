using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using _1_Scripts.Class;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ListReport : MonoBehaviour
{
    public GameObject scrollviewContent;
    public GameObject itemListReport;

    public void Call()
    {
        for (int i = 0; i < scrollviewContent.transform.childCount; i++)
        {
            Destroy(scrollviewContent.transform.GetChild(i).gameObject);
        }
        StartCoroutine(ServerSend());
    }



    IEnumerator ServerSend()
    {
        //Création url envoie serveur
        string url = Adressing.GetReportByUserUrl();

        //Récupération des infos user
        var idUser = UnityEngine.PlayerPrefs.GetString("userId");

        //Création du formulaire de donnée
        WWWForm form = new WWWForm();
        form.AddField("idUser", idUser);

        //Envoie des données au serveur
        UnityWebRequest www = UnityWebRequest.Post(url, form);

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
            var responseJson = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data, 3,
                www.downloadHandler.data.Length - 3);
            Debug.Log("Json response :" + responseJson);

            //Vérification de la réponse json 
            if (responseJson != "")
            {

                var listReport = JsonConvert.DeserializeObject<IEnumerable<Report>>(responseJson);
                float posY = 0;
                GameObject currentItem;

                
                //affichage des donnés dans l'UI
                foreach (var report in listReport)
                {
                    posY -= 80;
                    
                    //instanciation du prefab
                    currentItem = Instantiate(itemListReport, scrollviewContent.transform);
                    
                    //placement du prefab
                    currentItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(40, posY);
                    
                    //récupération des game objects du prefab
                    GameObject currentTextReportName =
                        currentItem.transform.Find("Title_ReportName/Label").gameObject;
                    GameObject currentButtonDownload =
                        currentItem.transform.Find("Button_download").gameObject;

                    
                    if (report.pathReport.Contains(".png"))
                    {
                        //valorisation des textes des game objects du prefab
                        currentTextReportName.GetComponent<Text>().text = "Capture whiteboard du " + report.dateReport;
                    }

                    if (report.pathReport.Contains(".pptx"))
                    {
                        //valorisation des textes des game objects du prefab
                        currentTextReportName.GetComponent<Text>().text = "Whiteboard du " + report.dateReport;
                    }
                    
                    if (report.pathReport.Contains(".pdf"))
                    {
                        //valorisation des textes des game objects du prefab
                        currentTextReportName.GetComponent<Text>().text = "Compte rendu du " + report.dateReport;
                    }
                    
                    //bouton rejoindre une salle
                    currentButtonDownload.gameObject.GetComponent<Button>().onClick.AddListener(delegate()
                    {
                        Process.Start(Path.Combine(Application.dataPath,report.pathReport));
                    });
                    
                    

                }
            }
        }
    }
}
