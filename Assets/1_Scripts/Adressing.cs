using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adressing : MonoBehaviour
{
    public static string ipAddress = "192.168.0.106";

    public static string GetSignInUrl()
    {
        string url = "http://" + ipAddress + "/edsa-ecvr/signin.php";
        return url;
    }
    
    public static string GetSignUpUrl()
    {
        string url = "http://" + ipAddress + "/edsa-ecvr/signup.php";
        return url;
    }
    
    public static string GetSignUpUrl_ConditionUniqueUserName()
    {
        string url = "http://" + ipAddress + "/edsa-ecvr/conditionUniqueUserName.php";
        return url;
    }
    
    public static string GetCreateRoomUrl()
    {
        string url = "http://" + ipAddress + "/edsa-ecvr/createRoom.php";
        return url;
    }
    
    public static string GetAllUsersUrl()
    {
        string url = "http://" + ipAddress + "/edsa-ecvr/getAllUsers.php";
        return url;
    }
}
