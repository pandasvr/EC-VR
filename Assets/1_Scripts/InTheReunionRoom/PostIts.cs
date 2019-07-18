using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using VRTK;

public class PostIts : MonoBehaviour
{
    public PhotonView photonView;
    
    //prefabs de post-its
    public GameObject postItJaune;
    public GameObject postItBleu;
    public GameObject postItRouge;
    public GameObject postItVert;

    //variables servant à étudier la manette
    protected VRTK_InteractGrab grabbingController;
    protected bool rightControllerExists;
    
    //Booléen servant à ne pas créer plus d'un post-it à la fois
    protected bool isObjectOnController;

    protected GameObject[] RightControllerChildrens; 
    
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
            }
        }
    }

    /*PunRPC
    public void synchronisationPostIt()
    {
        photonView.RPC("gerneratePostIt", RpcTarget.All);
    }*/
    
    public void gerneratePostIt()
    {
        //condition pour ne pas créer plus d'un post-it à la fois
        if (!isObjectOnController)
        {
            //si on a trouvé la position de la main, alors on créée le post-it rattaché à celle-ci
            if (rightControllerExists) 
            {
                GameObject postIt = Instantiate(postItJaune);
                postIt.transform.position = grabbingController.gameObject.transform.position+ new Vector3(0,0.08f,0);
                postIt.transform.rotation = new Quaternion(90.0f,0.0f,0.0f, 90.0f);
                grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(postIt);
                grabbingController.AttemptGrab();
                
                //postIts.transform.parent = rightControllerTransform;
                
                

                //Il y a maintenant un post-it sur la manette, on passe ce bool à true.
                isObjectOnController = true;
            }
        }
    }
    
    
    public void deletePostIt()
    {
        //condition pour ne pas créer plus d'un post-it à la fois
        if (isObjectOnController)
        {
            //si on a trouvé la position de la main, alors on créée le post-it rattaché à celle-ci
            if (rightControllerExists)
            {
                for (int i = 0; i < grabbingController.transform.childCount;)
                {
                    if (grabbingController.transform.GetChild(i).tag == "postit")
                    {
                        Destroy(grabbingController.transform.GetChild(i));
                    }
                }
                //Il n'y a maintenant plus de post-it sur la manette, on passe ce bool à false.
                isObjectOnController = false;
            }
        }
    }
    
}
