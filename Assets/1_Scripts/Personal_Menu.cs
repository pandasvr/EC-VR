using System.Collections;
using System.Collections.Generic;
using UnityEngine;

   public class Personal_Menu: MonoBehaviour
    {    
        
        public GameObject personnalMenuPanel;

        
        // Update is called once per frame
        public void ActivePersonalMenu()
        {
            personnalMenuPanel.SetActive(true);
            personnalMenuPanel.gameObject.SetActive(true);
        }
        
        
    }