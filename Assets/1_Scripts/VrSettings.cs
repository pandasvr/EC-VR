using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
 
public class VrSettings : MonoBehaviour {
 
    // Stopper la VR
    public void StopVR () {
        Screen.orientation = ScreenOrientation.AutoRotation ;
        XRSettings.enabled = false;
    }
    
    // Activer la VR
    public void StartVR () {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        XRSettings.enabled = true;
    }
}