using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MediaShare2 : MonoBehaviour
{
    public GameObject VideoProjecteur;
    public Image ImageProjecteur;
    public GameObject RadialMenuProjecteur;
    
    private VideoPlayer Video;
    private bool videoIsOn;
    
    private bool imageIsOn;
    private Sprite image;
    private int pageNumber;
    
    public void VideoButton() //cette fonction permet que l'appui sur le bouton "vidéo" lance la vidéo enregistrée dans l'objet videoplayer
    {
        videoIsOn = !videoIsOn; //lorsque le bouton est cliqué, le bouléen "on met la vidéo comme média" change de valeur
        VideoProjecteur.SetActive(videoIsOn); //si la vidéo est mise comme média, on active son support
        if (videoIsOn)
        {
            Video.Play(); //si la vidéo est mise comme média, on active son support, on la met en play
        }
    }
    
    public void ImageButton()
    {
        imageIsOn = !imageIsOn;
        ImageProjecteur.gameObject.SetActive(imageIsOn);
        RadialMenuProjecteur.SetActive(imageIsOn);
        if (imageIsOn)
        {
            ImageProjecteur.sprite = Resources.Load <Sprite> ("MediaShare/Presentation0");
            pageNumber = 0;
        }
    }

    public void updatePresentation()
    {
        var path = "MediaShare/Presentation" + pageNumber;
        ImageProjecteur.sprite = Resources.Load <Sprite> (path);
    }
    
    public void PresentatationSwipeRight()
    {
        if (pageNumber != 7)
        {
            pageNumber++;
            updatePresentation(); 
        }
    } 
    
    public void PresentatationSwipeLeft()
    {
        if (pageNumber != 0)
        {
            pageNumber--;
            updatePresentation();
        }
    }
}
