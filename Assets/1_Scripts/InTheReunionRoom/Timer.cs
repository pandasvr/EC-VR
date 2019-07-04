using System.Collections;
using System.Collections.Generic;
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

    private string currentTimeText;
    private bool isTimerEnd;
    private bool isBlinkinkStart;
    
    void Start ()
    {
        isTimerEnd = false;
        
        StartCoroutine("UpdateTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
    }
    void Update ()
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
    }
    
    IEnumerator UpdateTime()
    {
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
            
            if(timerMinutes == 0 && timerSecondes == 0)
            {
                isTimerEnd = true;
                timerText.color = Color.red;
            }
        }

        while (isTimerEnd)
        {
            yield return new WaitForSeconds (0.4f);
            timerText.enabled = !timerText.enabled;
        }
    }
}
