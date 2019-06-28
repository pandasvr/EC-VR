using UnityEngine;
using UnityEngine.UI;

public class initProfil : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField Field_UserName;
    public InputField Field_UserEmail;
    public Text Text_UserLvl;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Field_UserName.text = UnityEngine.PlayerPrefs.GetString("userName");
        Field_UserEmail.text = UnityEngine.PlayerPrefs.GetString("userEmail");
        Text_UserLvl.text = UnityEngine.PlayerPrefs.GetString("labelUserLevel");
    }
}
