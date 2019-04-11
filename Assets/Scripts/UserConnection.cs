using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserConnection : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField fieldName;
    public InputField fieldPassword;

    public void CallConnection()
    {
        StartCoroutine(Connection());
    }

    IEnumerator Connection()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", fieldName.text);
        form.AddField("password", fieldPassword.text);
        
        Debug.Log("nameField :" + fieldName.text);
        Debug.Log("passwordField :" + fieldPassword.text);

        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1/edsa-unitySQL/connection.php", form);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Post request complete!" + " Response Code: " + www.responseCode);
            string responseText = www.downloadHandler.text;
            Debug.Log("Response Text:" + responseText);
            
            if (responseText == "false")
            {
                Debug.Log("wrong name or password");
            }
            if(responseText == "true")
            {
                Debug.Log("user connected");
            }
            
        }
    }
}
