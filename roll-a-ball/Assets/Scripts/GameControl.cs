using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour
{
    public int collected;
    private int collectedMax;

    public int score;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI collectedText;
    public TextMeshProUGUI scoreText;

    public bool TimerIsRunning;
    public float P;
    public float maxP;

    public GameObject pickups;

    // Start is called before the first frame update
    void Start()
    {
        maxP = 60;
        P = maxP;
        collected = 0;
        collectedMax = pickups.transform.childCount;
        TimerIsRunning = true;
        setTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerIsRunning)
        {
            setTimerText();
            setCollectedText();
            setScoreText();

            if (P > 0)
            {
                P -= Time.deltaTime;
            }
            else
            {
                TimerIsRunning = false;
                P = 0;
            }
        }
    }

    void setCollectedText()
    {
        //Draw timer text to screen to tenth of second.
        collectedText.text = "Bamboo: " + collected.ToString() + " / " + collectedMax.ToString();
        if (collected == collectedMax)
        {
            TimerIsRunning = false;
            score += Convert.ToInt32(Mathf.Round(P*100));
        }
    }

    void setTimerText()
    {
        //Draw timer text to screen to tenth of second.
        timerText.text = P.ToString("F1");
    }

    void setScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
