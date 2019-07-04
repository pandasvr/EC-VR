using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PostIts : MonoBehaviour
{
    public GameObject postItJaune;
    public GameObject postItBleu;
    public GameObject postItRouge;
    public GameObject postItVert;

    protected Transform leftControllerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gerneratePostIt()
    {
        if (leftControllerTransform != null)
        {
            GameObject postIts = Instantiate(postItJaune);
            postIts.transform.parent = leftControllerTransform;
            postIts.transform.position = leftControllerTransform.position;
           Debug.Log("success");
        }
        else
        {
            try
            {
                leftControllerTransform = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.LeftController)
                    .gameObject
                    .transform;
                
            }
            catch (NullReferenceException){}
            Debug.Log("fail");
        }
    }
}
