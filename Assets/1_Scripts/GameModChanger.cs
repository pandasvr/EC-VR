using UnityEngine;

public class GameModChanger : MonoBehaviour
{
    public GameObject UIVR;
    public GameObject UIDesktop;
    public GameObject VRTK_SDKManager;
    public GameObject CameraDesktop;
    
    
    private bool isVRActive;

    // Start is called before the first frame update
    void Start()
    {
        //Récupération des variables
        VRTK_SDKManager = GameObject.FindGameObjectWithTag("VRTKManager");
        
        //Initialisation de l'UI en mode VR
        isVRActive = true;
        VRTK_SDKManager.SetActive(true);
        UIVR.SetActive(true);
        UIDesktop.SetActive(false);
        CameraDesktop.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown("space"))
        {
            if (isVRActive) 
            {
                Debug.Log("Desactivation mode VR");
                VRTK_SDKManager.SetActive(false);
                UIVR.SetActive(false);
                UIDesktop.SetActive(true);
                CameraDesktop.SetActive(true);

                //Libère et rend visible la souris
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                //Desactivation du Mode VR
                isVRActive = false;
            }
            else
            {
                Debug.Log("Activation mode VR");
                VRTK_SDKManager.SetActive(true);
                VRTK_SDKManager.SetActive(true);
                UIVR.SetActive(true);
                UIDesktop.SetActive(false);
                CameraDesktop.SetActive(false);

                //Cache et bloque la souris
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                //Desactivation du Mode VR
                isVRActive = true;
            }
           
        }
    }
}
