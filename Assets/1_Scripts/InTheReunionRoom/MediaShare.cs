using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MediaShare : MonoBehaviour
{
    protected SteamVR_TrackedController controller;
    public GameObject projectionScreen;
    protected VideoPlayer Video;
    protected bool videoIsOn= false;
    
        // Start is called before the first frame update
    void Start()
    {
        Video = projectionScreen.gameObject.GetComponent<VideoPlayer>();   
    }
    
    public void VideoButton()
    {
        videoIsOn = !videoIsOn;
        Video.gameObject.SetActive(videoIsOn);
        if (videoIsOn)
        {
            Video.Play();
        }
        
    }
   
}
