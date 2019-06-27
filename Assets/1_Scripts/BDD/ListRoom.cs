using System;
using System.Collections;
using System.Collections.Generic;
using _1_Scripts.Class;
using Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Room = _1_Scripts.Class.Room;

public class ListRoom : MonoBehaviour
{
    public GameObject scrollviewContent;
    public GameObject itemListRoom;


    private bool isCreatingRoom = false;
    private bool isJoiningRoom = false;
    private bool isConnecting;
    private const string GameVersion = "0.1";
    
    // Start is called before the first frame update
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
        List<Room> listRoom = new List<Room>();
        
        //Création url envoie serveur
        string urlListRoom_GetAllRoomsOfUser = Adressing.GetListRoomUrl_GetAllRoomsOfUser();

        //Récupération des infos user
        var idUser = UnityEngine.PlayerPrefs.GetString("userId");

        //Création du formulaire de donnée
        WWWForm formRoomOfUser = new WWWForm();
        formRoomOfUser.AddField("idUser", idUser);

        //Envoie des données au serveur
        UnityWebRequest wwwRoomOfUser = UnityWebRequest.Post(urlListRoom_GetAllRoomsOfUser, formRoomOfUser);

        //Récupération du retour serveur
        wwwRoomOfUser.downloadHandler = new DownloadHandlerBuffer();
        yield return wwwRoomOfUser.SendWebRequest();


        //Vérification du retour
        if (wwwRoomOfUser.isNetworkError || wwwRoomOfUser.isHttpError)
        {
            Debug.Log(wwwRoomOfUser.error);
        }
        else
        {
            Debug.Log("Post request complete!" + " Response Code: " + wwwRoomOfUser.responseCode);
            var responseJsonGetRoomOfUser = System.Text.Encoding.UTF8.GetString(wwwRoomOfUser.downloadHandler.data, 3,
                wwwRoomOfUser.downloadHandler.data.Length - 3);
            Debug.Log("Json response :" + responseJsonGetRoomOfUser);

            //Vérification de la réponse json 
            if (responseJsonGetRoomOfUser != "")
            {

                var listRoomOfUser = JsonConvert.DeserializeObject<IEnumerable<RoomOfUser>>(responseJsonGetRoomOfUser);
                float posY = 0;
                GameObject currentItem;

                //boucle sur la liste de toutes les salles d'un utilisateur
                foreach (var roomOfUser in listRoomOfUser)
                {
                    
                    //Création url envoie serveur
                    string urlListRoom_GetRoom = Adressing.GetListRoomUrl_GetRoom();

                    //Création du formulaire de donnée
                    WWWForm formGetRoom = new WWWForm();
                    formGetRoom.AddField("idRoom", roomOfUser.idRoom);

                    //Envoie des données au serveur
                    UnityWebRequest wwwGetRoom = UnityWebRequest.Post(urlListRoom_GetRoom, formGetRoom);

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
                            var tempListRoom = JsonConvert.DeserializeObject<IEnumerable<Room>>(responseJsonGetRoom);
                            
                            //boucle pour créer la liste des infos de chaque salle
                            foreach (var room in tempListRoom)
                            {
                                listRoom.Add(room);
                            }
                        }
                    }
                }

                //tri des salles (admin en premier)
                for (int i = 0; i < listRoom.Count; i++)
                {
                    if (listRoom[i].userCreatorName == UnityEngine.PlayerPrefs.GetString("userName"))
                    {
                        listRoom.Insert(0,listRoom[i]);
                        listRoom.RemoveAt(i+1);
                    }
                }

                //affichage des donnés dans l'UI
                foreach (var room in listRoom)
                {
                    posY -= 80;
                    
                    //instanciation du prefab
                    currentItem = Instantiate(itemListRoom, scrollviewContent.transform);
                    
                    //placement du prefab
                    currentItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(-40, posY);
                    
                    //récupération des game objects du prefab
                    GameObject currentTextNameRoom =
                        currentItem.transform.Find("Title_RoomName/Label").gameObject;
                    GameObject currentTextCreatorName =
                        currentItem.transform.Find("Title_CreatorName/Label").gameObject;
                    GameObject currentButtonJoinRoom =
                        currentItem.transform.Find("Button_joinRoom").gameObject;
                    
                    //bouton rejoindre une salle
                    currentButtonJoinRoom.gameObject.GetComponent<Button>().onClick.AddListener(delegate()
                    {
                        NetworkConnectManager.CreateNewRoom(room.roomName, room.maxPlayerRoom);
                        UnityEngine.PlayerPrefs.SetString("idRoom", room.idRoom);
                    });
                    
                    //valorisation des textes des game objects du prefab
                    currentTextNameRoom.GetComponent<Text>().text = room.roomName;
                    currentTextCreatorName.GetComponent<Text>().text = room.userCreatorName;

                    //activation du bouton modifier si on est le créateur de la salle
                    if (room.userCreatorName == UnityEngine.PlayerPrefs.GetString("userName"))
                    {
                        GameObject currentButtonModify =
                            currentItem.transform.Find("Button_modify").gameObject;
                        currentButtonModify.SetActive(true);
                    }
                }
            }
        }
    }


}
