using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using  VRTK;
public class FreezeEraser : MonoBehaviour
{
    private GameObject whiteboard;
    private GameObject eraser;
    private bool eraserIsGrabbed;
    public Rigidbody eraser_rigidbody;
    private bool whiteboardsIsFound;
    private bool isNearWhiteboard;

    private VRTK_InteractGrab grabbingController;
    private bool rightControllerExists;
    
    // Start is called before the first frame update
    void Start()
    {
        eraserIsGrabbed = false;
        whiteboardsIsFound = false;
        rightControllerExists = false;
    }

    // Update is called once per frame
    void Update()
    {
        /////////////////
        //Controller
        if (grabbingController == null) 
        {
            try
            {
                grabbingController = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.RightController)
                    .gameObject.GetComponent<VRTK_InteractGrab>();
                //on indique qu'on a trouvé la manette
                rightControllerExists = true;
            }
            catch (NullReferenceException){}
        } 
        ///////////////
        //Whiteboard
        if (whiteboard == null)
            {
                try
                {
                    whiteboard = GameObject.FindWithTag("Whiteboard");
                    whiteboardsIsFound = true;
                }
                catch (NullReferenceException)
                {
                }
            }
        ///////////////
        /// Eraser
        if (eraser == null)
        {
            try
            {
                eraser = GameObject.FindGameObjectWithTag("Eraser");
                Debug.Log("eraser found!");
            }
            catch (NullReferenceException)
            {
            }
        }

        if (rightControllerExists)
        {
            if (grabbingController.GetGrabbedObject() != null )
            {
                eraserIsGrabbed = (grabbingController.GetGrabbedObject().tag == "Eraser"); 
            }
            else
            {
                eraserIsGrabbed = false;
            }
        }

        if (whiteboardsIsFound)
        {
            //Debug.Log("markerIsGrabbed value is "+markerIsGrabbed);
            
            if (eraserIsGrabbed)
            {
                Debug.Log("reached here!");
                float distance = Vector3.Distance(eraser.transform.position, whiteboard.transform.position);
                isNearWhiteboard = true;//(distance < 3);
                if (isNearWhiteboard)
                {
                    Quaternion target = Quaternion.Euler(130, 180, 0);
                    Debug.Log("it should have worked!");
                    //eraser.transform.rotation = target;
                    eraser_rigidbody = eraser.GetComponent<Rigidbody>();
                    eraser_rigidbody.rotation *= target * grabbingController.transform.rotation;//Quaternion.Euler(-grabbingController.transform.rotation.x,grabbingController.transform.rotation.y, grabbingController.transform.rotation.z );
                    eraser_rigidbody.freezeRotation = true;
                    Debug.Log("until the end...");
                    
                }
                else
                {
                    eraser.GetComponent<Rigidbody>().freezeRotation = false;
                    Debug.Log("well, it didn't work");
                }
            }
        }
        
    }
};

