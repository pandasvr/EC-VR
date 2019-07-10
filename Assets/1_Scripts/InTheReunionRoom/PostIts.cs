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
    protected bool isObjectInController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //au lancement de l'application, on n'a pas encore trouvé la manette
        rightControllerExists = false;
        isObjectInController = false;
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
                isObjectInController = false;
            }
        }
    }

    //PunRPC
    public void synchronisationPostIt()
    {
        photonView.RPC("gerneratePostIt", RpcTarget.All);
    }
    
    [PunRPC]
    private void gerneratePostIt()
    {
        //condition pour ne pas créer plus d'un post-it à la fois
        if (!isObjectInController)
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
                isObjectInController = true;
            }
        }
    }
    
    
}
