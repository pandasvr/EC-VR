using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MediaShare : MonoBehaviour
{
    protected SteamVR_TrackedController controller;
    public GameObject projectionScreen;
    
    //variables servant à la présentation de la video
    protected VideoPlayer Video;
    protected bool videoIsOn= false;
    
    
    //variables servant à la présentation d'une suite d'images
    public Texture screenTurnedOffTexture;
    public Texture screenTurnedOnTexture;
    protected Renderer ScreenRenderer;
    protected bool presentationIsOn= false;
    
    
    
        // Start is called before the first frame update
    void Start()
    {
        Video = projectionScreen.gameObject.GetComponent<VideoPlayer>();  
        ScreenRenderer = projectionScreen.GetComponent<Renderer>();
        
    }
    
    
    
    public void VideoButton()
    {
        videoIsOn = !videoIsOn;
        projectionScreen.SetActive(videoIsOn);
        if (videoIsOn)
        {
            Video.Play();
        }
        
    }
    
    public void PresentationButton()
    {
        presentationIsOn = !presentationIsOn;
        projectionScreen.SetActive(presentationIsOn);
        //Texture.gameObject.SetActive(presentationIsOn);
        if (presentationIsOn)
        {
            ScreenRenderer.material.SetTexture("Presentation", screenTurnedOnTexture );
        }
        else
        {
            ScreenRenderer.material.SetTexture("defaultTexture", screenTurnedOffTexture ); //si on rappuie sur le bouton, on remet la texture "éteinte"
        }  
    }
   
}
