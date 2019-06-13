using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class RadialMenuSettings : MonoBehaviour
{
    [Header("Couleur des Icones")]
    public Color hoverEnterIconColor;
    public Color hoverExitIconColor;

    [Header("Les Menus")]
    public GameObject contextualMenu;
    public GameObject exitMenu;
    public GameObject playersListMenu;
    public GameObject radialMenuPanel;

    
    public GameObject handPointer;

    [Header("Pen Controller Model")] 
    public GameObject penModel;

    private bool isPenModelActive;
    private bool isPlayersListActive;

    private VRTK_Pointer pointerScript;
    private VRTK_UIPointer uiPointerScript;
    private VRTK_StraightPointerRenderer straightPointerScript;

    private VRTK_RadialMenuController radialMenuScript;

    public void Start()
    {
        //récupération des différents components dans le radialMenu et le controller hand
        radialMenuScript = this.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<VRTK_RadialMenuController>();
        pointerScript = handPointer.GetComponent<VRTK_Pointer>();
        uiPointerScript = handPointer.GetComponent<VRTK_UIPointer>();
        straightPointerScript = handPointer.GetComponent<VRTK_StraightPointerRenderer>();

        //initialisation des Bool
        isPenModelActive = false;
        isPlayersListActive = false;
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
        //on ferme le MenuPlayerList si il est activé
        if (isPlayersListActive)
        {
            ChangePlayersListMenuState();
        }
        
        ChangeUiPointerState(true);
        radialMenuScript.enabled = false;

        radialMenuPanel.GetComponent<Animator>().SetInteger("isDisable", 1);
        contextualMenu.GetComponent<Animator>().SetInteger("isEnable", 1);
    }

    //Desactivation du menu contextuel
    public void CloseContextualMenu()
    {
        ChangeUiPointerState(false);
        radialMenuScript.enabled = true;
        
        contextualMenu.GetComponent<Animator>().SetInteger("isEnable", 0);
        radialMenuPanel.GetComponent<Animator>().SetInteger("isDisable", 0);

    }
    
    //Activation du menu exit
    public void OpenExitMenu()
    {
        //on ferme le MenuPlayerList si il est activé
        if (isPlayersListActive)
        {
            ChangePlayersListMenuState();
        }
        
        ChangeUiPointerState(true);
        radialMenuScript.enabled = false;

        radialMenuPanel.GetComponent<Animator>().SetInteger("isDisable", 1);
        exitMenu.GetComponent<Animator>().SetInteger("isEnable", 1);
    }

    //Desactivation du menu exit
    public void CloseExitMenu()
    {
        ChangeUiPointerState(false);
        radialMenuScript.enabled = true;

        exitMenu.GetComponent<Animator>().SetInteger("isEnable", 0);
        radialMenuPanel.GetComponent<Animator>().SetInteger("isDisable", 0);

    }

    //permet de changer le model du controller Right
    public void ChangeRightControllerModel()
    {
        isPenModelActive = !isPenModelActive;
        
        ChangeUiPointerState(isPenModelActive);
        
        GameObject controller = GameObject.FindWithTag("controllerRight");
        controller.transform.Find("Model").gameObject.SetActive(!isPenModelActive);
        controller.transform.Find("pencil").gameObject.SetActive(isPenModelActive);

    }
    
    public void ChangePlayersListMenuState()
    {
        isPlayersListActive = !isPlayersListActive;
        int isActive;
        if (isPlayersListActive)
        {
            isActive = 1;
        }
        else
        {
            isActive = 0;
        }
        
        ChangeUiPointerState(isPlayersListActive);
        playersListMenu.GetComponent<Animator>().SetInteger("isEnable",isActive);
        

    }

    //Change le type de pointer entre le bezier (pour la téléportation) et le straight (pointer UI)
    // 0 = Bezier || 1 = straight
    private void ChangeUiPointerState(bool isActive)
    {
        pointerScript.enabled = isActive;
        straightPointerScript.enabled = isActive;
        uiPointerScript.enabled = isActive;
    }
    
}
