using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefs : MonoBehaviour
{
    public static void SaveUser(string userName, string userEmail, string userLevel, string labelUserLevel)
    {
        UnityEngine.PlayerPrefs.SetString("userName", userName);
        UnityEngine.PlayerPrefs.SetString("userEmail", userEmail);
        UnityEngine.PlayerPrefs.SetString("userLevel", userLevel);
        UnityEngine.PlayerPrefs.SetString("labelUserLevel", labelUserLevel);
    }

    public void Disconnect()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
        Debug.Log("User disconnected");
    }

    public static bool CheckUserConnected()
    {
        bool rt;
        
        if (UnityEngine.PlayerPrefs.HasKey("userName"))
        {
            rt = true;
        }
        else
        {
            rt = false;
        }

        return rt;
    }
}
