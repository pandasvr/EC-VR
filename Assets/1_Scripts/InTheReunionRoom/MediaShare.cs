using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;

public class MediaShare : MonoBehaviour
{
    public GameObject videoProjecteur;
    public Image imageProjecteur;
    public GameObject radialMenuProjecteur;
    public GameObject radialMenu;

    public PhotonView photonView;
    
    private VideoPlayer video;
    private bool videoIsOn;
    
    private bool imageIsOn;
    private Sprite image;
    private int pageNumber;

    private void Start()
    {
        imageIsOn = false;
        //on récupère le videoplayer qu'on voudra allumer où éteindre à partir d'une UI
        video = videoProjecteur.gameObject.GetComponent<VideoPlayer>();
        
    }



    public void SynchronisationVideo()
    {
        Debug.Log( " Video played send for all player.");
        photonView.RPC("PlayVideo", RpcTarget.All);
    }
    
    
    [PunRPC]
    //cette fonction permet que l'appui sur le bouton "vidéo" lance la vidéo enregistrée dans l'objet videoplayer
    private void PlayVideo(PhotonMessageInfo info) 
    {
        //lorsque le bouton est cliqué, le bouléen "on met la vidéo comme média" change de valeur
        //si la vidéo est mise comme média, on active son support
        //si la vidéo est mise comme média, on la met en play
        
        videoIsOn = !videoIsOn; 
        videoProjecteur.SetActive(videoIsOn); 
        radialMenu.SetActive(!videoIsOn);
        imageProjecteur.gameObject.SetActive(false);
        if (videoIsOn)
        {
            video.Play(); 
        }
        Debug.Log(string.Format("Info: {0} {1} {2}", info.Sender, info.photonView, info.timestamp));
    }
    
    
    
    public void SynchronisationPowerpoint()
    {
        Debug.Log( " Powerpoint started send for all player.");
        photonView.RPC("StartPowerPoint", RpcTarget.All);
    }
    
    [PunRPC]
    private void StartPowerPoint(PhotonMessageInfo info)
    {
        imageIsOn = !imageIsOn;
        videoProjecteur.SetActive(false);
        imageProjecteur.gameObject.SetActive(imageIsOn);
        radialMenuProjecteur.SetActive(imageIsOn);
        radialMenu.SetActive(!imageIsOn);
        if (imageIsOn)
        {
            imageProjecteur.sprite = Resources.Load <Sprite> ("MediaShare/Presentation0");
            pageNumber = 0;
        }
        Debug.Log(string.Format("Info: {0} {1} {2}", info.Sender, info.photonView, info.timestamp));
    }

    public void SynchronisationSwipe(string swipe)
    {
        if (swipe == "right")
        {
            Debug.Log(swipe + " swipe send for all player.");
            photonView.RPC("SwipeRight", RpcTarget.All);
        }
        else if (swipe == "left")
        {
            Debug.Log(swipe + " swipe send for all player.");
            photonView.RPC("SwipeLeft", RpcTarget.All);
        }
    }

    [PunRPC]
    private void SwipeRight(PhotonMessageInfo info)
    {
        if (pageNumber != 7)
        {
            pageNumber++;
            var path = "MediaShare/Presentation" + pageNumber;
            imageProjecteur.sprite = Resources.Load <Sprite> (path);
            
            Debug.Log(string.Format("Info: {0} {1} {2}", info.Sender, info.photonView, info.timestamp));
        }
    } 
    
    [PunRPC]
    private void SwipeLeft(PhotonMessageInfo info)
    {
        if (pageNumber != 0)
        {
            pageNumber--;
            var path = "MediaShare/Presentation" + pageNumber;
            imageProjecteur.sprite = Resources.Load <Sprite> (path);
            
            Debug.Log(string.Format("Info: {0} {1} {2}", info.Sender, info.photonView, info.timestamp));
        }
    }
}
