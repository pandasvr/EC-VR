using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.UIElements;
using VRTK;
using Image = UnityEngine.UI.Image;

public class MarkerColor : MonoBehaviour
{
    public GameObject[] ButtonIcons;
    private int colorIteration;
    public VRTK_InteractGrab grabbingController;
    public bool rightControllerExists;

    protected Color color;
    
    
    // Start is called before the first frame update
    void Start()
    {
        colorIteration = 0;
        ButtonIcons[colorIteration].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("ButtonIcons[3]" + ButtonIcons[3].GetComponent<Image>().color );

    }

    public void changeColor()
    {
        if (grabbingController.GetGrabbedObject().tag == "Marker")
        {
            colorIteration++;
            if (colorIteration == 4)
            {
                colorIteration = 0;
            }
            
            GameObject marker = grabbingController.GetGrabbedObject();
            
            for (int i = 0; i < ButtonIcons.Length; i++)
            {
                if (i == colorIteration)
                {
                    ButtonIcons[i].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    color = ButtonIcons[i].GetComponent<Image>().color;
                }
                else
                {
                    ButtonIcons[i].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                }
                Debug.Log("changed pic size");
            }

            foreach (Transform child in marker.transform)
            {
                if (child.tag == "markerColouredParts")
                {
                    child.GetComponent<MeshRenderer>().material.color = color;
                }
            }
        }
        
    }
}
