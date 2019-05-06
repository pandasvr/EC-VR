﻿using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

namespace Networking
{
    public class NetworkConnectManager :  MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields


        #endregion


        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        private const string GameVersion = "0.1";
        
        private VrSettings scriptVrSettings;
        private bool isCreatingRoom = false;
        private bool isJoiningRoom = false;

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        public byte maxPlayersPerRoom = 4;

        #endregion
        
        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        public void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            

            
            //TODO: Méthode de déconnexion de la VR à revoir
            //Deconnecte la VR par défaut sur la scène
            scriptVrSettings = this.GetComponent<VrSettings>();
            scriptVrSettings.StopVR();
            
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        private void Start()
        {
        }


        #endregion


        #region Public Methods

        
        public void CreateNewRoom()
        {
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = true;
            
            //L'utilisateur tente de créer une room
            isCreatingRoom = true;
            
            //TODO : Récupérer le vrai Identifiant de l'utilisateur
            //On donne un identifiant Random à l'utilisateur qui se connecte
            if (PhotonNetwork.NickName == "")
            {
                PhotonNetwork.NickName = playerName();
                Debug.Log("Identifiant temporaire de l'utilisateur : " + PhotonNetwork.NickName);
            }
            
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                //Création d'un nouveau salon
                var roomName = RoomName();
                isCreatingRoom = false;
                PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
                Debug.Log("Création du salon : " + roomName);
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                Debug.Log("#Critical, we must first and foremost connect to Photon Online Server.");
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }  
        }

        //TODO : Faire en sorte que le script rejoindre un salon défini par l'utilisateur
        //Script permettant de se connecter à un salon disponible aléatoirement
        public void JoinRoomSelected()
        {
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = true;
            
            //L'utilisateur tente de rejoindre une room
            isJoiningRoom = true;
            
            //TODO : Récupérer le vrai Identifiant de l'utilisateur
            //On donne un identifiant Random à l'utilisateur qui se connecte
            if (PhotonNetwork.NickName == "")
            {
                PhotonNetwork.NickName = playerName();
                Debug.Log("Identifiant temporaire de l'utilisateur : " + PhotonNetwork.NickName);
            }
            
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                //Rejoin un salon aléatoire disponible
                isJoiningRoom = false;
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                Debug.Log("#Critical, we must first and foremost connect to Photon Online Server.");
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }  
        }
        
        #endregion
        
        private string playerName()
        {
            return "Player#" + Random.Range(1, 9999);
        }

        private string RoomName()
        {
            return "Salon#" + Random.Range(1, 9999);
        }
        
        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                if (isJoiningRoom == true)
                {
                    //Retour à la phase "Rejoindre un salon"
                    JoinRoomSelected();
                }
                else if (isCreatingRoom == true)
                {
                    //Retour à la phase de création de salon
                    CreateNewRoom();
                }
            }   
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }


        #endregion
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available.");
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Chargement de la scene 'Salon 1' ");
                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("Salon 2");
            }
        }
    }
}