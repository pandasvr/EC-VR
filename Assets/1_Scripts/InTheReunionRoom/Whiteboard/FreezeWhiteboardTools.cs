
    using System;
    using UnityEngine;
    using VRTK;

    public class FreezeWhiteboardTools : MonoBehaviour
    {
        private GameObject whiteboard;
        private GameObject marker;
        private GameObject eraser;
        private bool markerIsGrabbed;
        private bool eraserIsGrabbed;
        private bool isNearWhiteboard;

        void Update()
        {
            if (whiteboard == null)
            {
                try
                {
                    whiteboard = GameObject.FindWithTag("Whiteboard");
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
            if (eraser == null)
            {
                try
                {
                    marker = GameObject.FindWithTag("Eraser");
                }
                catch (NullReferenceException)
                {
                }
            }


            markerIsGrabbed = marker.GetComponent<VRTK_InteractableObject>().IsGrabbed();
            eraserIsGrabbed = eraser.GetComponent<VRTK_InteractableObject>().IsGrabbed();

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
            if (eraserIsGrabbed)
            {
                float distance = Vector3.Distance(eraser.transform.position, whiteboard.transform.position);

                isNearWhiteboard = (distance < 3);

                if (isNearWhiteboard)
                {
                    Quaternion target = Quaternion.Euler(0, 180, 0);
                    eraser.GetComponent<Rigidbody>().rotation = target;
                    eraser.GetComponent<Rigidbody>().freezeRotation = true;
                }
                else
                {
                    eraser.GetComponent<Rigidbody>().freezeRotation = false;
                }
            }
        }
    }
