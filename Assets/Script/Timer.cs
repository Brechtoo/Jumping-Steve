using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 0f;
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public static float finalTime;


    // Start is called before the first frame update
    void Start()
    {
        timeIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeIsRunning)
        {
            if(timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
            }
        }
    }
    
    void DisplayTime (float timeToDisplay)
    {
        //timeToDisplay += 1;
        float minutes = Mathf.FloorToInt (timeToDisplay / 60);
        float seconds = Mathf.FloorToInt (timeToDisplay % 60);
        float milliseconds = (timeToDisplay % 1) * 1000;

        timeText.text = string.Format ("{0:0}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}
