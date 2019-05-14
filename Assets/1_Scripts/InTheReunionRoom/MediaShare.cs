using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MediaShare : MonoBehaviour
{
    protected SteamVR_TrackedController controller;
    public GameObject projectionScreen;
    protected VideoPlayer Video;
    
        // Start is called before the first frame update
    void Start()
    {
        Video = projectionScreen.gameObject.GetComponent<VideoPlayer>();   
    }
    
    public void VideoButton()
    {
        //Video.enable();
        Video.Play();
    }
    /*
    void jpgButton()
    {
        projectionScreen.GetComponent<>()
    }*/
}
