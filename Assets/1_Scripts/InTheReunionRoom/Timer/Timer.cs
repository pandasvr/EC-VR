using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    [Header("Set Timer")]
    [Tooltip("Nombre entre 0 et 99")]
    public int timerMinutes;
    [Tooltip("Nombre entre 0 et 59")]
    public int timerSecondes = 30;

    [Header("Set Timer Menu")] 
    public Dropdown dropdownTime;

    private string currentTimeText;
    private bool isTimerEnd;
    private bool isBlinkinkStart;
    private bool isTimerRunning;
    
    private PhotonView photonView;
    
    void Start ()
    {
        isTimerEnd = false;
        isTimerRunning = false;

        photonView = GetComponent<PhotonView>();
    }
    
    void Update ()
    {
        if (!isTimerEnd)
        {
            if (timerMinutes < 10)
            {
                currentTimeText = "0" + timerMinutes + ":";
            }
            else
            {
                currentTimeText = timerMinutes + ":";
            }

            if (timerSecondes < 10)
            {
                currentTimeText += "0" + timerSecondes;
            }
            else
            {
                currentTimeText += timerSecondes;
            }

            timerText.text = currentTimeText;
            
            if(timerMinutes == 0 && timerSecondes == 0)
            {
                isTimerEnd = true;
                timerText.color = Color.red;
            }
        }
    }

    public void RPCStartTimer()
    {
        photonView.RPC("StartTimer", RpcTarget.All);
    }
    
    public void RPCResetTimer()
    {
        int intValue = dropdownTime.value;
        string stringValue = dropdownTime.options[intValue].text;
        photonView.RPC("ResetTimer", RpcTarget.All, stringValue);
    }

    public void RPCPauseTimer()
    {
        photonView.RPC("PauseTimer", RpcTarget.All);
    }

    [PunRPC]
    private void StartTimer()
    {
        if (!isTimerRunning)
        {
            isTimerEnd = false;
            StartCoroutine("UpdateTime");
            Time.timeScale = 1; //Just making sure that the timeScale is right 
        }
    }

    [PunRPC]
    private void ResetTimer(string min)
    {
        if (isTimerRunning)
        {
            StopCoroutine("UpdateTime");
            isTimerRunning = false;
            timerText.color = Color.white; 
            if (timerText.enabled == false)
            {
                timerText.enabled = true;
            }   
        }   
        
        timerMinutes = int.Parse(min.Substring(0, 2));
        timerSecondes = 0;
    }
	
    [PunRPC]
    private void PauseTimer()
    {
        if (isTimerRunning)
        {
            StopCoroutine("UpdateTime");
            isTimerRunning = false;
        }
		
	}

    IEnumerator UpdateTime()
    {
        isTimerRunning = true;
        while (!isTimerEnd) {

            yield return new WaitForSeconds (1);
            if (timerSecondes == 0)
            {
                timerSecondes = 59;
                timerMinutes--;
            }
            else
            {
                timerSecondes--;
            }
            
        }

        while (isTimerEnd)
        {
            yield return new WaitForSeconds (0.4f);
            timerText.enabled = !timerText.enabled;
        }

        isTimerRunning = false;
    }
}
