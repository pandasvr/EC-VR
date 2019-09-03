using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuSettings : MonoBehaviour
{
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
                            anim.Play("Admin_to_Salon");
                            break;
                        case "options":
                            anim.Play("Options_to_Salon");
                            break;
                        case "profil":
                            anim.Play("Profil_to_Salon");
                            break;
                        case "joinRoom":
                            anim.Play("JoinRoom_to_Salon");
                            break;
                        /*case"salon":
                            anim.Play("Salon_to_Salon");
                            break;*/
                    }
                    break;
                case "admin":
                    switch (currentMenu)
                    {
                        case "salon":
                            anim.Play("Salon_to_Admin");
                            break;
                        case "options":
                            anim.Play("Options_to_Admin");
                            break;
                        case "profil":
                            anim.Play("Profil_to_Admin");
                            break;
                    }
                    break;
                case "options":
                    switch (currentMenu)
                    {
                        case "salon":
                            anim.Play("Salon_to_Options");
                            break;
                        case "admin":
                            anim.Play("Admin_to_Options");
                            break;
                        case "profil":
                            anim.Play("Profil_to_Options");
                            break;
                    }
                    break;
                case "profil":
                    switch (currentMenu)
                    {
                        case "salon":
                            anim.Play("Salon_to_Profil");
                            break;
                        case "admin":
                            anim.Play("Admin_to_Profil");
                            break;
                        case "options":
                            anim.Play("Options_to_Profil");
                            break;
                    }
                    break;
                
                case "createRoom":
                    anim.Play("Salon_to_CreateRoom");
                    break;
                
                case "joinRoom":
                    anim.Play("Salon_to_JoinRoom");
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
