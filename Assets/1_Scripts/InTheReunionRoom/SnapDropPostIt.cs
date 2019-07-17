using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SnapDropPostIt : MonoBehaviour
{
    //l'ombre du post-it indiquant où le post-it va se placer
    public GameObject shadowPostIt;
    
    private GameObject postItBoard;
    private VRTK_InteractableObject vrtkInteractableObject;
    private bool isGrabbed;
    private bool isToReplace;

    private void Start()
    {
        vrtkInteractableObject = gameObject.GetComponent<VRTK_InteractableObject>();
        isToReplace = true;
    }

    void Update()
    {
        if (postItBoard == null)
        {
            try
            {
                postItBoard = GameObject.FindWithTag("PostItBoard");
            }
            catch (NullReferenceException)
            {
            }
        }

        isGrabbed = vrtkInteractableObject.IsGrabbed();

        if (isGrabbed)
        {
            isToReplace = true;
            
            if (shadowPostIt.activeSelf)
            {
                shadowPostIt.transform.position = new Vector3(gameObject.transform.position.x,
                    gameObject.transform.position.y, postItBoard.transform.position.z + 0.052f);
                Quaternion target = Quaternion.Euler(-90f, 0, 180f);
                shadowPostIt.transform.rotation = target;
            }
        }
        

        if (!isGrabbed && isToReplace)
        {
            Debug.Log("placement du post-it...");
            isToReplace = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                gameObject.transform.position.y, postItBoard.transform.position.z + 0.052f);
            Quaternion target = Quaternion.Euler(-90f, 0, 180f);
            gameObject.transform.rotation = target;
            
            shadowPostIt.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == postItBoard && isGrabbed)
        {
            Debug.Log("Post_it dans la zone tableau !");
            vrtkInteractableObject.validDrop = VRTK_InteractableObject.ValidDropTypes.DropAnywhere;
            shadowPostIt.SetActive(true);        
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject == postItBoard && isGrabbed && !shadowPostIt.activeSelf)
        {
            shadowPostIt.SetActive(true);
        }
        
        /*
        if (collider.gameObject == postItBoard && isGrabbed)
        {
            //otherMethode
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, postItBoard.transform.position.z + 0.052f);
            Quaternion target = Quaternion.Euler(-90f, 0, 180f);
            gameObject.transform.rotation = target;
        }
        */

    }

    private void OnTriggerExit(Collider collider)
    {
        isGrabbed = vrtkInteractableObject.IsGrabbed();
        
        if (collider.gameObject == postItBoard && isGrabbed)
        {
            Debug.Log("Post_it hors de la zone tableau !");
            vrtkInteractableObject.validDrop = VRTK_InteractableObject.ValidDropTypes.NoDrop;
            shadowPostIt.SetActive(false);
        }
        
    }
}
