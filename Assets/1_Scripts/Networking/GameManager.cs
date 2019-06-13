﻿using System;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using VRTK;
using Random = UnityEngine.Random;

namespace Networking
{
        
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        
        public GameObject[] spawnPoints;
        public GameObject personalMenuCanvas;
        
        protected Vector3 previousPosition;
        protected Vector3 newPosition;
        protected float timer;
        protected Vector3 playerFirstPos;
        protected GameObject userPrefabInstantiation;
        
        private Transform headsetTransform;

        
        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                playerFirstPos = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;
                PhotonNetwork.Instantiate(this.playerPrefab.name, playerFirstPos, Quaternion.identity, 0);
            }

            //on initialise des données traitées dans l'update()
            previousPosition = playerFirstPos;
            newPosition = new Vector3(0,0,0);
            timer = 0.0f;

            /*GameObject[] userGameObjects = GameObject.FindGameObjectsWithTag("Avatar");
            foreach (GameObject userGameObject in userGameObjects)
            {
                if (PhotonView.Get(userGameObject).IsMine)
                {
                    userPrefabInstantiation = userGameObject;
                    break;
                }
            }*/
        }
        
        
        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {        
            SceneManager.LoadScene("MainMenu");
        }

        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
        
        #region Private Methods



        #endregion
        
        #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        }


        #endregion
        
        
        //fonction qui définit s'il y a un mouvement entre deux positions
        protected bool userMoves(Vector3 previousPosition, Vector3 newposition)
        {
            return (Vector3.Magnitude(previousPosition - newposition) <= 0.01f);
        }
        
        //Fonction qui change la valeur du statut de l'utilisateur, selon depuis combien de temps il n'a pas bougé (passage de connecté à absent)
        //où selon s'il est dans un menu
        protected void updateStatus(float timer, Vector3 previousPosition, Vector3 newPosition, out float updatedTimer)
        {
            //le string auquel nous allons attribuer unee valeur selon certaines conditions
            string status = "connecté";
            //on attribue la valeur en sortie du timer au cas où elle ne serait pas changée
            updatedTimer = timer;
            
            if (userMoves(previousPosition, newPosition))
            {
                updatedTimer = 0;
                if (personalMenuCanvas.activeInHierarchy)
                {
                    status = "dans le menu";
                }
                else
                {
                    status = "connecté";
                }
            }
            
            //Tout d'abord, la condition pour savoir si l'utilisateur est absent
            //condition sur un timer
            if (timer == 2)
            {
                status = "absent";
            }
            Debug.Log(status);
            UnityEngine.PlayerPrefs.SetString("userStatus", status);
        }
        
        

        private void Update()
        {
            //dans l'update, on va évaluer le statut de l'utilisateur, 
            //pour celà, on va réévaluer l'ancienne position previousPosition et la nouvelle newPosition
            //On pourra ensuite les traiter à l'aide de la fonction updateStatus, qui nous permettra de savoir avoec le timer si l'utilisateur est absent, connecté, où dans le menu
            
            previousPosition = newPosition; 
            if (headsetTransform != null)
            {
                newPosition = headsetTransform.position;
            }
            else
            {
                try
                {
                    headsetTransform = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset).gameObject
                        .transform;
                    newPosition = headsetTransform.position;
                }
                catch (NullReferenceException)
                {
                    newPosition = Vector3.zero;
                }
            }

            //Avec les lignes suivantes, on fait évoluer le timer qui compte la durée de non-mouvement de l'utilisateur, et on fait évoluer le statut
            timer += Time.deltaTime;
            updateStatus(timer, previousPosition, newPosition, out timer);
            
            Debug.Log(UnityEngine.PlayerPrefs.GetString("userStatus"));
            
        }
    }
    
    
    
}
