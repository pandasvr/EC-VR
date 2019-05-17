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
    public Renderer ScreenRenderer;
    protected  Shader ScreenShader;
    protected bool presentationIsOn= false;

    public List<Texture> presentationSlides;// liste d'images (slides) de la presentation, pour l'instant vide
    protected Texture image; //varible servant à remplir PresentationSlides
    string myPath = "Assets/2_Images/MediaShare"; //chemin où seront rangés tous les médias
    string presentationPath="/Ppt/PresentationSlides"; //chemin de notre présentation en particulier
    private string imagePath; //variable qui servira à transformer les objets images en textures à partir de leur adresse dans le projet
    protected int PresentationPageNumber = 0; //nombre de pages
    protected int PresentationPageNumberSet = 0; //numéro de page qui sera modifié par l'utilisateur
    
    
        // Start is called before the first frame update
    void Start()
    {
        Video = projectionScreen.gameObject.GetComponent<VideoPlayer>();  //on récupère le videoplayer qu'on voudra allumer où éteindre à partir d'une UI
        ScreenRenderer = GetComponent<Renderer>(); //on récupère le renderer de l'écran qu'on voudra allumer où éteindre à partir d'une UI, en en changeant la texture
        ScreenRenderer.enabled = true;
        ScreenShader = projectionScreen.GetComponent<Shader>();
        
        
        foreach (string sFileName in System.IO.Directory.GetFiles(myPath+presentationPath))//on parcourt les documents situés dans le dossier de la présentation
        {
            Debug.Log(sFileName);
            if (System.IO.Path.GetExtension(sFileName) == ".PNG")// si c'est une image
            {
                imagePath = myPath + presentationPath + "sFileName";
                image = Resources.Load(imagePath) as Texture ; //on la charge en tant qu'objet unity
                presentationSlides.Add(image) ;//on l'ajoute à la présentation
            }
        }
        
        PresentationPageNumber = presentationSlides.Count;//on compte les pages de la presentation
        Debug.Log(PresentationPageNumber);
    }
    
    
    
    public void VideoButton() //cette fonction permet que l'appui sur le bouton "vidéo" lance la vidéo enregistrée dans l'objet videoplayer
    {
        videoIsOn = !videoIsOn; //lorsque le bouton est cliqué, le bouléen "on met la vidéo comme média" change de valeur
        projectionScreen.SetActive(videoIsOn); //si la vidéo est mise comme média, on active son support
        if (videoIsOn)
        {
            Video.Play(); //si la vidéo est mise comme média, on active son support, on la met en play
        }
    }
    
    public void PresentationButton()
    {
        Debug.Log("PresentationButton"+PresentationPageNumberSet);
        presentationIsOn = !presentationIsOn;
        projectionScreen.SetActive(presentationIsOn);
        //Texture.gameObject.SetActive(presentationIsOn);
        if (presentationIsOn)
        {
            ScreenRenderer.material= new Material(ScreenShader);
            ScreenRenderer.material.SetTexture("Presentation", presentationSlides[PresentationPageNumberSet] );
        //    ScreenRenderer.material.mainTexture= presentationSlides[PresentationPageNumberSet] ;
            
        }
        else
        {
            ScreenRenderer.material.SetTexture("defaultTexture", presentationSlides[PresentationPageNumberSet] ); //si on rappuie sur le bouton, on remet la texture "éteinte"
        }  
    }

    
    public void PresentatationSwipeRight()
    {
        PresentationPageNumberSet++;
        Debug.Log("PresentatationSwipeRight"+PresentationPageNumber);
        PresentationUpdate();
    } 
    
    public void PresentatationSwipeLeft()
    {
        PresentationPageNumberSet--;
        PresentationUpdate();
    }

    public void PresentationUpdate()
    {
        PresentationPageNumberSet = PresentationPageNumberSet % PresentationPageNumber;
        if (PresentationPageNumberSet < 0) {PresentationPageNumberSet +=PresentationPageNumber;}
        
        Debug.Log(PresentationPageNumberSet);
        
        ScreenRenderer.material.SetTexture("PrersentationPage",presentationSlides[PresentationPageNumberSet]);
    }
   
}
