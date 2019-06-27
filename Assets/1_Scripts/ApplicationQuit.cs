using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    public void DoQuitApp()
    {
        Debug.Log("L'utilisateur a quitté l'application.");
        Application.Quit();
    }
}
