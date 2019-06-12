using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class RadialMenuSettings : MonoBehaviour
{
    public Color hoverEnterIconColor;
    public Color hoverExitIconColor;

    public GameObject contextualMenu;
    public GameObject exitMenu;
    public GameObject radialMenuPanel;
    
    public VRTK_Pointer handPointer;

    [Header("Pen Controller Model")] 
    public GameObject penModel;

    private bool isPenModelActive;

    private VRTK_RadialMenuController radialMenuScript;

    public void Start()
    {
        radialMenuScript = this.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<VRTK_RadialMenuController>();
        isPenModelActive = false;
    }

    //Change la couleur de l'icone en Hover Enter
    public void HoverEnterColorIcon(Image icon)
    {
        icon.color = new Color();
        icon.color = hoverEnterIconColor;
    }
    
    //Change la couleur de l'icone en Hover Exit
    public void HoverExitColorIcon(Image icon)
    {
        icon.color = new Color();
        icon.color = hoverExitIconColor;
    }

    //Effectue une animation de l'Arc button en Hover Enter
    public void HoverEnterAnimationArc(Animator anim)
    {
        anim.SetInteger("isEnter", 1);
    }
    
    //Effectue une animation de l'Arc button en Hover Exit

    public void HoverExitAnimationArc(Animator anim)
    {
        anim.SetInteger("isEnter", 0); 
    }

    //Activation du menu contextuel
    public void OpenContextualMenu()
    {
        handPointer.enabled = true;
        radialMenuScript.enabled = false;

        radialMenuPanel.GetComponent<Animator>().SetInteger("isDisable", 1);
        contextualMenu.GetComponent<Animator>().SetInteger("isEnable", 1);
    }

    //Desactivation du menu contextuel
    public void CloseContextualMenu()
    {
        handPointer.enabled = false;
        radialMenuScript.enabled = true;
        
        contextualMenu.GetComponent<Animator>().SetInteger("isEnable", 0);
        radialMenuPanel.GetComponent<Animator>().SetInteger("isDisable", 0);

    }
    
    //Activation du menu exit
    public void OpenExitMenu()
    {
        handPointer.enabled = true;
        radialMenuScript.enabled = false;

        radialMenuPanel.GetComponent<Animator>().SetInteger("isDisable", 1);
        exitMenu.GetComponent<Animator>().SetInteger("isEnable", 1);
    }

    //Desactivation du menu exit
    public void CloseExitMenu()
    {
        handPointer.enabled = false;
        radialMenuScript.enabled = true;

        exitMenu.GetComponent<Animator>().SetInteger("isEnable", 0);
        radialMenuPanel.GetComponent<Animator>().SetInteger("isDisable", 0);

    }

    //permet de changer le model du controller Left
    public void ChangeLeftControllerModel()
    {
        isPenModelActive = !isPenModelActive;
        
        GameObject controller = GameObject.FindWithTag("controllerLeft");
        controller.transform.Find("Model").gameObject.SetActive(!isPenModelActive);
        controller.transform.Find("pencil").gameObject.SetActive(isPenModelActive);

    }
}
