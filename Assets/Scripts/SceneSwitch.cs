using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    public void SceneSwitcher_GoToTest()
    {
        SceneManager.LoadScene("RoomTest");

    }
    
    public void SceneSwitcher_GoToMainPanel()
    {
        SceneManager.LoadScene("MainMenu");

    }
}
