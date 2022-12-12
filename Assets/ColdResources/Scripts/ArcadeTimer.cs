using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class ArcadeTimer : MonoBehaviour
{
    
    [SerializeField] private TMP_Text timerTextP1; 
    [SerializeField] private TMP_Text timerTextP2;
    [SerializeField] private TMP_Text startTimerText;
    
    [SerializeField] private VoidGameEvent startTimeOutEvent;
    [SerializeField] private VoidGameEvent timerOutEvent;
    
    private float startTimer = 4; //3sec
    // public float StartTimer => startTimer;
    private bool startTimeOut = false;
    public bool StartTimeOut => startTimeOut;
    
    [SerializeField] private float raceTime = 180; //3min
    private float timer; //3min
    private bool timeOut = false;

    private void Awake() {
        timer = raceTime;
    }

    void Update () {
        if (!startTimeOut) {
            startTimer -= Time.deltaTime;
            int cooldown = Mathf.FloorToInt(startTimer % 60f);
            startTimerText.text = cooldown.ToString("0");
            if (startTimer <= 0) {
                startTimeOut = true;
                startTimerText.gameObject.SetActive(false);
                startTimeOutEvent.Call();
            }
        }
        if (startTimeOut && !timeOut) {
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            int milliseconds = Mathf.FloorToInt((timer * 100f) % 100f);
            timerTextP1.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" +
                                milliseconds.ToString("00");
            timerTextP2.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" +
                                milliseconds.ToString("00");
            if (timer <= 0) {
                timeOut = true;
                timerTextP2.text = "00:00:00";
                timerOutEvent.Call();
            }
        }
    }

}
