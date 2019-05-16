using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkConnection : MonoBehaviour
{
    public GameObject PanelLogin;
    public GameObject PanelMain;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.CheckUserConnected())
        {
            PanelLogin.SetActive(true);
            PanelMain.SetActive(false);
        }
    }
}
