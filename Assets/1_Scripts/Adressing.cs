using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adressing : MonoBehaviour
{
    public string ipAddress;
    private static string ipAddressStatic;

    private void Start()
    {
        ipAddressStatic = ipAddress;
    }

    public static string GetSignInUrl()
    {
        string url = "http://" + ipAddressStatic + "/edsa-ecvr/signin.php";
        return url;
    }
    
    public static string GetSignUpUrl()
    {
        string url = "http://" + ipAddressStatic + "/edsa-ecvr/signup.php";
        return url;
    }
    
    public static string GetSignUpUrl_ConditionUniqueUserName()
    {
        string url = "http://" + ipAddressStatic + "/edsa-ecvr/conditionUniqueUserName.php";
        return url;
    }
    
    public static string GetCreateRoomUrl()
    {
        string url = "http://" + ipAddressStatic + "/edsa-ecvr/createRoom.php";
        return url;
    }
    
    public static string GetAllUsersUrl()
    {
        string url = "http://" + ipAddressStatic + "/edsa-ecvr/getAllUsers.php";
        return url;
    }

    public static string GetCreateInviteUrl()
    {
        string url = "http://" + ipAddressStatic + "/edsa-ecvr/createInvite.php";
        return url;
    }
    
    public static string GetListRoomUrl_GetAllRoomsOfUser()
    {
        string url = "http://" + ipAddressStatic + "/edsa-ecvr/getAllRoomsOfUser.php";
        return url;
    }
    
    public static string GetListRoomUrl_GetRoom()
    {
        string url = "http://" + ipAddressStatic + "/edsa-ecvr/getRoom.php";
        return url;
    }
}
