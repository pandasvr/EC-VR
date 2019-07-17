using System.Collections;
using _1_Scripts.Class;
using Newtonsoft.Json;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

public class Init_Panel_MenuPrincipal : MonoBehaviour
{
    public GameObject Button_Parameters;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckCreatorId());
    }

    IEnumerator CheckCreatorId()
    {
        //Récupération du nom de la room
        var roomName = PhotonNetwork.CurrentRoom.Name;
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
                var actualRoom = JsonConvert.DeserializeObject<Room>(responseJsonGetRoom);

                if (actualRoom.userCreatorName == UnityEngine.PlayerPrefs.GetString("userName"))
                {
                    Button_Parameters.SetActive(true);
                }
                else
                {
                    Button_Parameters.SetActive(false);
                }
            }
        }
    }
}
