
    using System;
    using UnityEngine;
    using VRTK;

    public class FreezeMarker
    {
        private GameObject whiteboard;
        private GameObject marker;
        private bool isGrabbed;
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

            isGrabbed = marker.GetComponent<VRTK_InteractableObject>().IsGrabbed();

            if (isGrabbed)
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
