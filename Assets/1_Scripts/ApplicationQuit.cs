using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    public void DoQuitApp()
    {
        Debug.Log("L'utilisateur a quitté l'application.");
        Application.Quit();
    }
}
