using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefs : MonoBehaviour
{
    public InputField Field_UserName;
    public InputField Field_UserEmail;
    
    // Start is called before the first frame update
    void Start()
    {
        Field_UserName.text = UnityEngine.PlayerPrefs.GetString("userName");
        Field_UserEmail.text = UnityEngine.PlayerPrefs.GetString("userEmail");
    }

    public static void SaveUser(string userName, string userEmail, string userLevel)
    {
        UnityEngine.PlayerPrefs.SetString("userName", userName);
        UnityEngine.PlayerPrefs.SetString("userEmail", userEmail);
        UnityEngine.PlayerPrefs.SetString("userLevel", userLevel);
    }
}
