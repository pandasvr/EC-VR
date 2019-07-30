using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using  VRTK;
using VRTK.GrabAttachMechanics;

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
            catch (NullReferenceException){}
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
            catch (NullReferenceException){}
        }
        
        //on regarde sur la manette si elle exite, la présence de la gomme
        if (rightControllerExists)
        {
            if (grabbingController.GetGrabbedObject() != null )
            {
                eraserIsGrabbed = (grabbingController.GetGrabbedObject().tag == "Eraser"); 
            }
            //s'il n'y a pas d'objet sur la manette il n'y a pas de gomme
            else 
            {
                eraserIsGrabbed = false;
            }
        }
        
        //si le whiteboard existe
        if (whiteboardsIsFound)
        {
            //si la gomme est dans la main
            if (eraserIsGrabbed)
            {
                //on calcule sa distance avec le tableau
                float distance = Vector3.Distance(eraser.transform.position, whiteboard.transform.position);
                //si elle est proche du tableau
                isNearWhiteboard = (distance < 3);
                if (isNearWhiteboard)
                {
                    //on change sa rotation
                    Quaternion target = Quaternion.Euler(0, 180, 0);
                    eraser_rigidbody = eraser.GetComponent<Rigidbody>();
                    eraser_rigidbody.rotation = target;
                    eraser_rigidbody.freezeRotation = true;
                }
                else
                {
                    //Sinon, elle suit la manette
                    eraser_rigidbody.freezeRotation = false;
                }
            }
        }
        
    }
};

