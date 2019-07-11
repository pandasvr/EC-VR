﻿using System.Collections;
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

    [Header("Set Timer Menu")] 
    public Dropdown dropdownTime;

    private string currentTimeText;
    private bool isTimerEnd;
    private bool isBlinkinkStart;
    private bool isTimerRunning;
    
    void Start ()
    {
        isTimerEnd = false;
        isTimerRunning = false;
    }
    
    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            StartCoroutine("UpdateTime");
            Time.timeScale = 1; //Just making sure that the timeScale is right 
        }
    }

    public void SetTime()
    {
        if (isTimerRunning)
        {
            ResetTimer();
        }
        
        int intValue = dropdownTime.value;
        string stringValue = dropdownTime.options[intValue].text;
        timerMinutes = int.Parse(stringValue.Substring(0, 2));
        timerSecondes = 0;
    }

    public void ResetTimer()
    {
        StopCoroutine("UpdateTime");
        isTimerEnd = false;
        timerText.text = "00:00";
        timerText.color = Color.white;
        if (timerText.enabled == false)
        {
            timerText.enabled = true;
        }
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
