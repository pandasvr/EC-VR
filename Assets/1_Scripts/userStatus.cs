using System;
using Photon.Pun;
using UnityEngine;
using VRTK;

public class userStatus : MonoBehaviour
{
    public GameObject statusSphere;
    public PhotonView photonView;
    public Material ConnectedStatutMaterial;
    public Material AbsentStatutMaterial;
    public Material InTheMenuStatutMaterial;
    public float timer;
        
    protected Vector3 previousPosition;
    protected Vector3 newPosition;
    protected Vector3 playerFirstPos;
    protected string status;
    protected string previousStatus;
    protected Material currentMaterial;

    private Renderer statusSphereRenderer;
    private Transform headsetTransform;
    
    
    void Start()
    {
        //on initialise des données qui seront traitées dans l'update()
        previousPosition = playerFirstPos;
        newPosition = new Vector3(0,0,0);
        timer = 0.0f;
        status = "status";
       
        statusSphereRenderer = statusSphere.gameObject.GetComponent<Renderer>();
    }
    
    
    //Fonction qui appelle la fonction de visualisation du statut
    public void synchronisationUserStatus()
    {
        photonView.RPC("updateStatusSphereMaterial", RpcTarget.All);
    }
    
    //Fonction de visualisation du statut
    //La fonction récupère le statuit et attribue une couleur à une sphere en fonction de celui-ci
    [PunRPC]
    private void updateStatusSphereMaterial()
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

        statusSphereRenderer.material = currentMaterial;
    }
    
    //Fonction qui définit s'il y a un l'utilisateur se déplace à partir de ses positions
    //en comparant la norme de la différence des vecteurs
    protected bool userMoves(Vector3 previousPosition, Vector3 newposition)
    {
       //on renvoie que userMoves vaut true si le déplacement est plus grand qu'une certaine valeur
        return (!(Vector3.Magnitude(previousPosition - newposition) <= 0.0005f));
    }
        
    
    //Fonction qui change la valeur du statut de l'utilisateur, selon depuis combien de temps il n'a pas bougé (passage de connecté à absent)
    //où selon s'il est dans un menu
    protected void updateStatus(float timer, Vector3 previousPosition, Vector3 newPosition, out float updatedTimer)
    {
        //le string sur lequel on va calculer le statut
        //Si l'utilisateur bouge, il est soit connecté soit dans un menu
        //Sinon, s'il ne bouge pas pendant plus de 120s, il est considéré absent
        //On attribue finalementà la playerpref userStatus sa nouvelle valeur

        string status = "connecté";
        updatedTimer = timer;
            
        if (userMoves(previousPosition, newPosition))
        {
            updatedTimer = 0;
            status = "connecté";
        }
        if (timer >= 5.0f)
        {
            status = "absent";
        }  
        UnityEngine.PlayerPrefs.SetString("userStatus", status);
    }


    
    void Update()
    {
        //dans l'update, on va évaluer le statut de l'utilisateur, 
        //pour celà, on va réévaluer l'ancienne position previousPosition et la nouvelle newPosition
        //On pourra ensuite les traiter à l'aide de la fonction updateStatus, qui nous permettra de
        //savoir avoec le timer si l'utilisateur est absent, connecté, où dans le menu
            
        previousPosition = newPosition; 
        if (headsetTransform != null)
        {
            newPosition = headsetTransform.position;
        }
        else
        {
            try
            {
                headsetTransform = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset).gameObject
                    .transform;
                newPosition = headsetTransform.position;
            }
            catch (NullReferenceException)
            {
                newPosition = Vector3.zero;
            }
        }

        //Avec les lignes suivantes, on fait évoluer le timer qui compte la durée de non-mouvement de l'utilisateur, et on fait évoluer le statut
        timer += Time.deltaTime;
        
        updateStatus(timer, previousPosition, newPosition, out timer); 
        
        previousStatus = status;
        status = UnityEngine.PlayerPrefs.GetString("userStatus");
        synchronisationUserStatus();
    }
}

        //on attribue la valeur en sortie du timer au cas où elle ne serait pas changée