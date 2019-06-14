using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class userStatus : MonoBehaviour
{
    public GameObject statusSphere;
    public PhotonView photonView;
    public Material ConnectedStatutMaterial;
    public Material AbsentStatutMaterial;
    public Material InTheMenuStatutMaterial;
    
    protected string status;
    protected string previousStatus;
    protected Material currentMaterial;

    private Renderer statusSphereRenderer;
    
    public void synchronisationUserStatus()
    {
        photonView.RPC("updateUserStatus", RpcTarget.All);
    }
    
    [PunRPC]
    private void updateUserStatus()
    {
        
        switch (status)
        {
            case "connecté":
                currentMaterial = ConnectedStatutMaterial;
                break;
            case "dans le menu":
                currentMaterial = InTheMenuStatutMaterial;
                break;
            case "absent":
                currentMaterial = AbsentStatutMaterial;
                break;
        }

        Debug.Log(currentMaterial);
        if (previousStatus != status)
        {
            statusSphereRenderer.material = currentMaterial;
        }
    }
    
    
    
    void Start()
    {
        statusSphereRenderer = statusSphere.gameObject.GetComponent<Renderer>();
    }

    
    void Update()
    {
        previousStatus = status;
        status = UnityEngine.PlayerPrefs.GetString("userStatus");
        Debug.Log(status);

        synchronisationUserStatus();
    }
}
