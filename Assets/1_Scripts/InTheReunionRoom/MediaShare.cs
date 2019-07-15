using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;

public class MediaShare : MonoBehaviour
{
    [Header(("Vidéo projecteur"))]
    public GameObject videoProjecteur;
    public Image imageProjecteur;

    [Header("Fake PowerPoint Slides")] 
    public Texture[] slides;

    [Header("Synchronisation Photon View")]
    public PhotonView photonView;
    
    private VideoPlayer video;
    private bool videoState;

    private Color offScreenProjecteur;
    
    private bool powerpointState;
    private Sprite image;
    private int pageNumber;
    private int pageNumberMax;

    private void Start()
    {
        //on désactive le statut du powerpoint et de la video
        powerpointState = false;
        videoState = false;
        //on récupère le videoplayer qu'on voudra allumer où éteindre à partir d'une UI
        video = videoProjecteur.gameObject.GetComponent<VideoPlayer>();
        //couleur du projecteur eteint
        offScreenProjecteur = Color.black;
        
    }

    public void SynchronisationVideo()
    {
        Debug.Log( " Video played send for all player.");
        photonView.RPC("PlayVideo", RpcTarget.All);
    }
    
    
    [PunRPC]
    //cette fonction permet que l'appui sur le bouton "vidéo" Lance ou Stop la vidéo selectionnée
    private void PlayVideo(PhotonMessageInfo info) 
    {     
        videoState = !videoState; 

        if (powerpointState)
        {
            powerpointState = false;
            radialMenuProjecteur.SetActive(false);
            videoProjecteur.GetComponent<Renderer>().material.SetTexture(null, null);
        }
        
        if (videoState)
        {
            video.Play();
            videoProjecteur.GetComponent<Renderer>().material.color = Color.white;
        }

        videoProjecteur.SetActive(videoIsOn); //si la vidéo est mise comme média, on active son support


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

        if (videoState)
        {
            videoState = false;
            video.Stop();
        }

        if (powerpointState)
        {
            videoProjecteur.GetComponent<Renderer>().material.color = Color.white;
            videoProjecteur.GetComponent<Renderer>().material.SetTexture("_MainTex", slides[0]);  
            pageNumber = 0;
        }
        else
        {
            videoProjecteur.GetComponent<Renderer>().material.SetTexture(null, null);
            videoProjecteur.GetComponent<Renderer>().material.color = offScreenProjecteur;
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
        if (pageNumber != slides.Length-1)
        {
            pageNumber++;
            videoProjecteur.GetComponent<Renderer>().material.SetTexture("_MainTex",  slides[pageNumber]);
            
            Debug.Log(string.Format("Info: {0} {1} {2}", info.Sender, info.photonView, info.timestamp));
        }
    } 
    
    [PunRPC]
    private void SwipeLeft(PhotonMessageInfo info)
    {
        if (pageNumber != 0)
        {
            pageNumber--;
            videoProjecteur.GetComponent<Renderer>().material.SetTexture("_MainTex",  slides[pageNumber]);
            
            Debug.Log(string.Format("Info: {0} {1} {2}", info.Sender, info.photonView, info.timestamp));
        }
    }
}
