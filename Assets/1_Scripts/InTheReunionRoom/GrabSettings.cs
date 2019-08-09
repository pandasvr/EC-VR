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
    public VRTK_InteractGrab grabbingController;
    protected bool rightControllerExists;
    
    //Booléen servant à traiter les objets présents sur la manette
    protected bool isObjectOnController;
    protected bool isPostItOnController;
    protected bool isWhiteboardToolOnController;

    private GameObject instantiatedEraser;
    private GameObject instantiatedMarker;
    
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
        try
        {
            instantiatedEraser.tag = "Eraser";
            
        }
        catch(MissingReferenceException){}
        catch(NullReferenceException){}
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
                    createTool(markerPrefab, out instantiatedMarker);
                    grabTool(instantiatedMarker);
                    //Il y a maintenant un post-it sur la manette, on passe ce bool à true.
                    isObjectOnController = true;
                }
            }
            else
            {
                createTool(markerPrefab, out instantiatedMarker);
                grabTool(instantiatedMarker);
                //Il y a maintenant un post-it sur la manette, on passe ce bool à true.
                isObjectOnController = true;
            }
            foreach (Transform child in instantiatedMarker.transform)
            {
                if (child.tag == "markerColouredParts")
                {
                    child.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
        }
    }

    public void createTool(GameObject prefab, out GameObject tool)
    {
        tool = Instantiate(prefab);
        tool.transform.position =
            grabbingController.gameObject.transform.position + new Vector3(0, -0.08f, 0);
        tool.transform.rotation = new Quaternion(90.0f, 0.0f, 0.0f, 90.0f);
        

    }

    public void grabTool(GameObject tool)
    {
        grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(tool);
        grabbingController.AttemptGrab();
        tool.transform.rotation = new Quaternion(90.0f, 0.0f, 0.0f, 90.0f);
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
                GameObject grabbedObject = grabbingController.GetGrabbedObject();
                grabbingController.ForceRelease();
                Destroy(grabbedObject);
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
                createTool(eraserPrefab, out instantiatedEraser);
                grabTool(instantiatedEraser);
                //Il y a maintenant un post-it sur la manette, on passe ce bool à true.
                isObjectOnController = true;
            }
        }
    }
    
    
    
}
