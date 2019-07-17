using UnityEngine;

public class PlayerPrefs : MonoBehaviour
{
    public static void SaveUser(string userId, string userName, string userEmail, string userLevel, string labelUserLevel, string userFirstName, string userLastName)
    {
        UnityEngine.PlayerPrefs.SetString("userId", userId);
        UnityEngine.PlayerPrefs.SetString("userName", userName);
        UnityEngine.PlayerPrefs.SetString("userEmail", userEmail);
        UnityEngine.PlayerPrefs.SetString("userLevel", userLevel);
        UnityEngine.PlayerPrefs.SetString("labelUserLevel", labelUserLevel);
        UnityEngine.PlayerPrefs.SetString("userFirstName", userFirstName);
        UnityEngine.PlayerPrefs.SetString("userLastName", userLastName);
        
        UnityEngine.PlayerPrefs.SetString("userStatus", "connecté");
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
