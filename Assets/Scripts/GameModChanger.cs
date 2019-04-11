using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GameModChanger : MonoBehaviour
{
    public GameObject NonVRCamera;
    public GameObject VRTK_SDKManager;
    public Canvas UICanvas;
    public VRTK_UICanvas UICanvasScript;
    private Vector3 ScaleTemp;
    private Vector3 PositionTemp;

    public bool isVRActive;

    // Start is called before the first frame update
    void Start()
    {
        isVRActive = true;
        VRTK_SDKManager.SetActive(true);
        NonVRCamera.SetActive(false);
        UICanvas.renderMode = RenderMode.WorldSpace;
        //Scale par defaut
        ScaleTemp = UICanvas.transform.localScale;
        PositionTemp = UICanvas.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (isVRActive)
            {
                Debug.Log("Desactivation mode VR");
                VRTK_SDKManager.SetActive(false);
                NonVRCamera.SetActive(true);
                UICanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                UICanvasScript.enabled = false;

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
                NonVRCamera.SetActive(false);
                UICanvas.renderMode = RenderMode.WorldSpace;
                UICanvasScript.enabled = true;
                UICanvas.transform.localScale = ScaleTemp;
                UICanvas.transform.localPosition = PositionTemp;

                //Cache et bloque la souris
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                //Desactivation du Mode VR
                isVRActive = true;
            }
           
        }
    }
}
