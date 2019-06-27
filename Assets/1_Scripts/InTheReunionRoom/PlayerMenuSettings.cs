using UnityEngine;

   public class PlayerMenuSettings: MonoBehaviour
   {
        public GameObject[] playerMenuPanels;
        public GameObject playerMenuCanvas;
        protected bool isMenuOn;

        //Le player menu est en statut Off par défaut
        void Start()
        {
            isMenuOn = false;
        }
        
        //On modifie le statut (on/off) du player Menu
        public void MenuState()
        {
            isMenuOn = !isMenuOn;

            //On desactive tout les panels lors de la désactivation du player Menu 
            if (!isMenuOn)
            {
                foreach (var panel in playerMenuPanels)
                {
                    panel.SetActive(false);
                }
                Debug.Log("Désactivation du player Menu");
            }
            else
            {
                //on active le premier panel de la liste
                playerMenuPanels[0].SetActive(true); 
                Debug.Log("Activation du player Menu");
            }
            
            playerMenuCanvas.SetActive(isMenuOn);
        }
        
    }