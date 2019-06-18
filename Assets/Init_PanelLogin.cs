using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_PanelLogin : MonoBehaviour
{
    public GameObject Panel_Login;
    public GameObject Panel_Main;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.CheckUserConnected())
        {
            Panel_Login.SetActive(false);
            Panel_Main.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
