
using System;
using UnityEngine;
using VRTK;

public class FreezeWhiteboardTools : MonoBehaviour
{
    private GameObject whiteboard;
    private GameObject marker;
    private bool markerIsGrabbed;
    private bool whiteboardsIsFound;
    private bool isNearWhiteboard;

    void Start()
    {
        markerIsGrabbed = false;
        whiteboardsIsFound = false;
    }
    
    void Update()
    {
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
        
        if (marker == null)
        {
            try
            {
                marker = GameObject.FindWithTag("Marker");
            }
            catch (NullReferenceException)
            {
            }
        }

        try
        {
            markerIsGrabbed = (marker.GetComponent<VRTK_InteractableObject>().IsGrabbed())&&(GameObject.FindGameObjectWithTag("Marker")!=null);
        }
        catch(NullReferenceException){}

        if (whiteboardsIsFound)
        {
            //Debug.Log("markerIsGrabbed value is "+markerIsGrabbed);
            if (markerIsGrabbed)
            {
                float distance = Vector3.Distance(marker.transform.position, whiteboard.transform.position);

                isNearWhiteboard = (distance < 3);

                if (isNearWhiteboard)
                {
                    Quaternion target = Quaternion.Euler(0, 180, 0);
                    marker.GetComponent<Rigidbody>().rotation = target;
                    marker.GetComponent<Rigidbody>().freezeRotation = true;
                }
                else
                {
                    marker.GetComponent<Rigidbody>().freezeRotation = false;
                }
            }
            
        }
        
    }
}
