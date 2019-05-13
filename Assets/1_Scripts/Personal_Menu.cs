using System.Collections;
using System.Collections.Generic;
using UnityEngine;

   public class Personal_Menu: MonoBehaviour
    {    
        
        public GameObject personnalMenuPanel;
        protected bool personalMenuOn = false;
        
        // Update is called once per frame
        public void ActivePersonalMenu()
        {
            personalMenuOn = !personalMenuOn;
            personnalMenuPanel.SetActive(personalMenuOn);
            personnalMenuPanel.gameObject.SetActive(personalMenuOn);
        }

       /* void Update()
        {
            if (Input.GetKeyDown(KeyCode.Home))
            {
                ActivePersonalMenu();
            }
        }*/
        
        
    }