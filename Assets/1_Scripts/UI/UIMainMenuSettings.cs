using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuSettings : MonoBehaviour
{
    public GameObject panel_Salon;
    public GameObject panel_Admin;
    public GameObject panel_Options;
    public GameObject panel_Profil;

    private Animator anim;
    private String currentMenu = "salon";
    private String menuToSwitch;
    private bool isMenuChange;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMenuChange)
        {
            switch (menuToSwitch)
            {
                case "salon":
                    switch (currentMenu)
                    {
                        case "admin":
                            panel_Admin.GetComponent<UIFade>().FadeOut();
                            panel_Salon.GetComponent<UIFade>().FadeIn();
                            anim.Play("Admin_to_Salon");
                            break;
                        case "options":
                            panel_Options.GetComponent<UIFade>().FadeOut();
                            panel_Salon.GetComponent<UIFade>().FadeIn();
                            anim.Play("Options_to_Salon");
                            break;
                        case "profil":
                            panel_Profil.GetComponent<UIFade>().FadeOut();
                            panel_Salon.GetComponent<UIFade>().FadeIn();
                            anim.Play("Profil_to_Salon");
                            break;
                    }
                    break;
                case "admin":
                    switch (currentMenu)
                    {
                        case "salon":
                            panel_Salon.GetComponent<UIFade>().FadeOut();
                            panel_Admin.GetComponent<UIFade>().FadeIn();
                            anim.Play("Salon_to_Admin");
                            break;
                        case "options":
                            panel_Options.GetComponent<UIFade>().FadeOut();
                            panel_Admin.GetComponent<UIFade>().FadeIn();
                            anim.Play("Options_to_Admin");
                            break;
                        case "profil":
                            panel_Profil.GetComponent<UIFade>().FadeOut();
                            panel_Admin.GetComponent<UIFade>().FadeIn();
                            anim.Play("Profil_to_Admin");
                            break;
                    }
                    break;
                case "options":
                    switch (currentMenu)
                    {
                        case "salon":
                            panel_Salon.GetComponent<UIFade>().FadeOut();
                            panel_Options.GetComponent<UIFade>().FadeIn();
                            anim.Play("Salon_to_Options");
                            break;
                        case "admin":
                            panel_Admin.GetComponent<UIFade>().FadeOut();
                            panel_Options.GetComponent<UIFade>().FadeIn();
                            anim.Play("Admin_to_Options");
                            break;
                        case "profil":
                            panel_Profil.GetComponent<UIFade>().FadeOut();
                            panel_Options.GetComponent<UIFade>().FadeIn();
                            anim.Play("Profil_to_Options");
                            break;
                    }
                    break;
                case "profil":
                    switch (currentMenu)
                    {
                        case "salon":
                            panel_Salon.GetComponent<UIFade>().FadeOut();
                            panel_Profil.GetComponent<UIFade>().FadeIn();
                            anim.Play("Salon_to_Profil");
                            break;
                        case "admin":
                            panel_Admin.GetComponent<UIFade>().FadeOut();
                            panel_Profil.GetComponent<UIFade>().FadeIn();
                            anim.Play("Admin_to_Profil");
                            break;
                        case "options":
                            panel_Options.GetComponent<UIFade>().FadeOut();
                            panel_Profil.GetComponent<UIFade>().FadeIn();
                            anim.Play("Options_to_Profil");
                            break;
                    }
                    break;
            }
            
            currentMenu = menuToSwitch;
            isMenuChange = false; 
        }
    }

    
    //choix entre : salon / admin / options / profil
    public void ChangeMenu(String menuName)
    {
        if (menuName != currentMenu)
        {
            menuToSwitch = menuName;
            isMenuChange = true;
        }
    }
}
