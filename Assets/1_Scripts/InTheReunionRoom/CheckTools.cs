using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTools : MonoBehaviour
{
    public GameObject whiteboard;

    public GameObject postIt;
    
    // Start is called before the first frame update
    void Start()
    {
        checkATool(UnityEngine.PlayerPrefs.GetString("whiteboard"), whiteboard);
        checkATool(UnityEngine.PlayerPrefs.GetString("postIt"), postIt);
    }

    public void checkATool(string tool, GameObject objet)
    {
        if (tool == "False")
        {
            objet.SetActive(false);
        }
        else
        {
            objet.SetActive(true);
        }
    }
}
