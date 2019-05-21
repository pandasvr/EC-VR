using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Photon.Pun;

public class LaserPointer :  MonoBehaviourPunCallbacks, IPunObservable
{
    private LineRenderer laserRenderer; //on travaille sur le line renderer selon s'il rencontre un objet sur sa trajectoire
    public GameObject laser;//on va se servir du gameobject pour changer sa position, ainsi que pour l'alluler où l'éteindre
    protected bool isLaserOn;
    public GameObject hand; //la main sur laquelle est placée le laser
    
    // Start is called before the first frame update
    void Start()
    {
        laserRenderer = GetComponent<LineRenderer>();
        laserRenderer.enabled = false;
        isLaserOn = false;
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isLaserOn);
        }
        else
        {
            isLaserOn = (bool) stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            if (isLaserOn && !laserRenderer.enabled)
            {
                laserRenderer.enabled = true; 
            }

            if (!isLaserOn && laserRenderer.enabled)
            {
                laserRenderer.enabled = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                isLaserOn = !isLaserOn;
                laserRenderer.enabled = isLaserOn;
            }
        
            if(laserRenderer.enabled)
            {
                laser.transform.position   = hand.transform.position;
                laser.transform.rotation   = hand.transform.rotation;
        
                laserRenderer.SetPosition(0,transform.position);
                RaycastHit hit;
        
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    laserRenderer.SetPosition(1, hit.point);
                }
                else
                {
                    laserRenderer.SetPosition(1, transform.forward * 50000);
                }
            }
        }           
    }

}
