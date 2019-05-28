using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;

public class MediaShare : MonoBehaviour
{
    public GameObject videoProjecteur;
    public Image imageProjecteur;
    public GameObject radialMenuProjecteur;

    public PhotonView photonView;
    
    private VideoPlayer video;
    private bool videoIsOn;
    
    private bool imageIsOn;
    private Sprite image;
    private int pageNumber;
    private int pageNumberMax;

    private void Start()
    {
        imageIsOn = false;
        //on récupère le videoplayer qu'on voudra allumer où éteindre à partir d'une UI
        video = videoProjecteur.gameObject.GetComponent<VideoPlayer>();
        //pageNumberMax = Directory.GetFiles("Assets/Resources/MediaShare", "*.jpg", SearchOption.TopDirectoryOnly).Length;
        pageNumberMax = 4;
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
        imageProjecteur.gameObject.SetActive(false);
        if (videoIsOn)
        {
            video.Play(); 
        }

        videoProjecteur.SetActive(videoIsOn); //si la vidéo est mise comme média, on active son support
        //radialMenu.SetActive(!videoIsOn);

        if (videoIsOn)
        {
            video.Play(); //si la vidéo est mise comme média, on active son support, on la met en play
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

        if (imageIsOn)
        {
            imageProjecteur.sprite = Resources.Load <Sprite> ("MediaShare/Presentation1");
            pageNumber = 1;
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
        if (pageNumber != pageNumberMax)
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
        if (pageNumber != 1)
        {
            pageNumber--;
            var path = "MediaShare/Presentation" + pageNumber;
            imageProjecteur.sprite = Resources.Load <Sprite> (path);
            
            Debug.Log(string.Format("Info: {0} {1} {2}", info.Sender, info.photonView, info.timestamp));
        }
    }
}
