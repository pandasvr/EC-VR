using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;

public class MediaShare : MonoBehaviour
{
    public GameObject projectionScreen;
    
    public GameObject VideoProjecteur;
    public Image ImageProjecteur;
    public GameObject RadialMenuProjecteur;
    public GameObject RadialMenu;
    
    private VideoPlayer Video;
    private bool videoIsOn;
    
    private bool imageIsOn = false;
    private Sprite image;
    private int pageNumber;

    private void Start()
    {
        Video = projectionScreen.gameObject.GetComponent<VideoPlayer>();  //on récupère le videoplayer qu'on voudra allumer où éteindre à partir d'une UI
    }

    public void VideoButton() //cette fonction permet que l'appui sur le bouton "vidéo" lance la vidéo enregistrée dans l'objet videoplayer
    {
        videoIsOn = !videoIsOn; //lorsque le bouton est cliqué, le bouléen "on met la vidéo comme média" change de valeur
        VideoProjecteur.SetActive(videoIsOn); //si la vidéo est mise comme média, on active son support
        RadialMenu.SetActive(!videoIsOn);
        ImageProjecteur.gameObject.SetActive(false);
        if (videoIsOn)
        {
            Video.Play(); //si la vidéo est mise comme média, on active son support, on la met en play
        }
    }
    
    public void ImageButton()
    {
        imageIsOn = !imageIsOn;
        VideoProjecteur.SetActive(false);
        ImageProjecteur.gameObject.SetActive(imageIsOn);
        RadialMenuProjecteur.SetActive(imageIsOn);
        RadialMenu.SetActive(!imageIsOn);
        if (imageIsOn)
        {
            ImageProjecteur.sprite = Resources.Load <Sprite> ("MediaShare/Presentation0");
            pageNumber = 0;
        }
    }

    
    public void PresentatationSwipeRight()
    {
        if (pageNumber != 7)
        {
            pageNumber++;
            var path = "MediaShare/Presentation" + pageNumber;
            ImageProjecteur.sprite = Resources.Load <Sprite> (path);
        }
        
    } 
    
    public void PresentatationSwipeLeft()
    {
        if (pageNumber != 0)
        {
            pageNumber--;
            var path = "MediaShare/Presentation" + pageNumber;
            ImageProjecteur.sprite = Resources.Load <Sprite> (path);
        }
    }
}
