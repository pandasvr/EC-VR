using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using VRTK;

public class GrabSettings : MonoBehaviour
{   
    //prefabs de post-its
    [Header("Fourniture Prefabs")]
    public GameObject[] postItPrefabs;
    public GameObject markerPrefab; 
    public GameObject eraserPrefab; 
    
    //RadialMenus
    [Header("Fourniture Radial Menu")]
    public GameObject radialMenuPostIt;
    public GameObject radialMenuMarker;

    //variables servant à étudier la manette
    protected VRTK_InteractGrab grabbingController;
    protected bool rightControllerExists;
    
    //Booléen servant à traiter les objets présents sur la manette
    protected bool isObjectOnController;
    protected bool isPostItOnController;
    protected bool isWhiteboardToolOnController;
    
    // Start is called before the first frame update
    void Start()
    {
        //au lancement de l'application, on n'a pas encore trouvé la manette
        rightControllerExists = false;
        isObjectOnController = false;
    }

    
    // Update is called once per frame
    void Update()
    {
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
        else
        {
            if (grabbingController.GetGrabbedObject() == null)
            {
                isObjectOnController = false;
                
                radialMenuMarker.SetActive(false);
                radialMenuPostIt.SetActive(false);
            }
            else 
            {
                isObjectOnController = true;
                if ((grabbingController.GetGrabbedObject().tag == "Marker")|(grabbingController.GetGrabbedObject().tag == "Eraser"))
                {
                    isWhiteboardToolOnController = true;
                }
                else
                {
                    isWhiteboardToolOnController = false;
                }
                
                if (grabbingController.GetGrabbedObject().tag == "postit")
                {
                    isPostItOnController = true;
                }
                else
                {
                    isPostItOnController = false;
                }
                radialMenuMarker.SetActive(isWhiteboardToolOnController);
                radialMenuPostIt.SetActive(isPostItOnController);
            }
        }
    }

    
    /// ///////////////////////
    /// Post-its
    /// //////////////////////
    /*PunRPC
    public void synchronisationPostIt()
    {
        GetComponent<PhotonView>().RPC("gerneratePostIt", RpcTarget.All);
    }*/
    
    public void gerneratePostIt()
    {
        //condition pour ne pas créer plus d'un post-it à la fois
        if (!isObjectOnController)
        {
            //si on a trouvé la position de la main, alors on créée le post-it rattaché à celle-ci
            if (rightControllerExists) 
            {
                GameObject postIt = Instantiate(postItPrefabs[0]);
                postIt.transform.position = grabbingController.gameObject.transform.position+ new Vector3(0,0.08f,0);
                postIt.transform.rotation = new Quaternion(90.0f,0.0f,0.0f, 90.0f);
                grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(postIt);
                grabbingController.AttemptGrab();
                
               //Il y a maintenant un post-it sur la manette, on passe ce bool à true.
                isObjectOnController = true;
            }
        }
    }
    
    /*PunRPC
    public void synchronizeDeletePostIt()
    {
        photonView.RPC("deletePostIt", RpcTarget.All);
    }*/
    
    public void deletePostIt()
    {
        //condition pour ne pas créer plus d'un post-it à la fois
        if (isObjectOnController)
        {
            //si on a trouvé la position de la main, alors on créée le post-it rattaché à celle-ci
            if (grabbingController.GetGrabbedObject().tag == "postit")
                {
                    Destroy(grabbingController.GetGrabbedObject());
                }
                //Il n'y a maintenant plus de post-it sur la manette, on passe ce bool à false.
                isObjectOnController = false;
        }
    }
    
    
    /// ///////////////////////////////////////
    /// Marqueurs
    /// /// //////////////////////
  
    /*PunRPC
    public void synchronisationMarker()
    {
        photonView.RPC("gernerateMarker", RpcTarget.All);
    }*/
    
    public void gernerateMarker()
    {
        //condition pour ne pas créer plus d'un post-it à la fois
        if (rightControllerExists)
        {
            if (grabbingController.GetGrabbedObject() != null)
            {
                if (grabbingController.GetGrabbedObject().tag == "Eraser")
                {
                    deleteWhiteboardTool();
                    createMarker();
                    //Il y a maintenant un post-it sur la manette, on passe ce bool à true.
                    isObjectOnController = true;
                }
            }
            else
            {
                Debug.Log("dis program reaches here too2");
                createMarker();
                //Il y a maintenant un post-it sur la manette, on passe ce bool à true.
                isObjectOnController = true;
            }

        }
    }

    public void createMarker()
    {
        GameObject instantiatedMarker = Instantiate(markerPrefab);
        instantiatedMarker.transform.position =
            grabbingController.gameObject.transform.position + new Vector3(0, 0.08f, 0);
        instantiatedMarker.transform.rotation = new Quaternion(90.0f, 0.0f, 0.0f, 90.0f);
        grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(instantiatedMarker);
        grabbingController.AttemptGrab();
        instantiatedMarker.transform.rotation = new Quaternion(90.0f, 0.0f, 0.0f, 90.0f);

    }
    
    /*PunRPC
    public void synchronizeDeleteMarker()
    {
        photonView.RPC("deleteMarker", RpcTarget.All);
    }*/
    
    public void deleteWhiteboardTool()
    {
        //condition pour ne pas créer plus d'un post-it à la fois
        if (isObjectOnController)
        {
            //si on a trouvé la position de la main, alors on créée le post-it rattaché à celle-ci
            if ((grabbingController.GetGrabbedObject().tag == "Marker")|(grabbingController.GetGrabbedObject().tag == "Eraser"))
            {
                Destroy(grabbingController.GetGrabbedObject());
            }
            //Il n'y a maintenant plus de post-it sur la manette, on passe ce bool à false.
            isObjectOnController = false;
        }
    }
    
    /// ///////////////////////////////////////
    /// Effaceur
    /// /// //////////////////////
  
    /*PunRPC
    public void synchronisationMarker()
    {
        photonView.RPC("gernerateMarker", RpcTarget.All);
    }*/
    
    public void gernerateEraser()
    {
        //condition pour ne pas créer plus d'un post-it à la fois
        if (rightControllerExists)
        {
            //si on a trouvé la position de la main, alors on créée le post-it rattaché à celle-ci
            if (grabbingController.GetGrabbedObject().tag == "Marker") 
            {
                deleteWhiteboardTool();
                GameObject instantiatedEraser = Instantiate(eraserPrefab);
                instantiatedEraser.transform.position = grabbingController.gameObject.transform.position;
                instantiatedEraser.transform.rotation = new Quaternion(90.0f,0.0f,0.0f, 90.0f);
                grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(instantiatedEraser);
                grabbingController.AttemptGrab();
                instantiatedEraser.transform.rotation = new Quaternion(90.0f,0.0f,0.0f, 90.0f);
                //Il y a maintenant un post-it sur la manette, on passe ce bool à true.
                isObjectOnController = true;
            }
        }
    }
    
    
    
}
