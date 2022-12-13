using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArcadeTimer : MonoBehaviour
{
    
    [SerializeField] private TMP_Text timerTextP1; 
    [SerializeField] private TMP_Text timerTextP2;
    [SerializeField] private Image startTimerImg;
    // [SerializeField] private TMP_Text startTimerText;
    
    [SerializeField] private VoidGameEvent startTimeOutEvent;
    
    [SerializeField] private ControllersDetection controllersDetection;
    
    [SerializeField] private Sprite[] starterSprites ;
    
    private float startTimer = 5; //3sec
    // public float StartTimer => startTimer;
    private bool startTimeOut = false;
    private bool goTimeOut = false;
    public bool GoTimeOut => startTimeOut;
    
    [SerializeField] private float raceTime = 180; //3min
    private float timer; //3min
    private bool timeOut = false;

    private void Awake() {
        timer = raceTime;
    }

    void Update () {
        if (!startTimeOut) {
            startTimer -= Time.deltaTime;
            
            // int cooldown = Mathf.FloorToInt(startTimer % 60f);
            // startTimerText.text = cooldown.ToString("0");

            int cooldown = Mathf.FloorToInt(startTimer % 60f);
            if (cooldown>=0 && cooldown<=3) {
                startTimerImg.sprite = starterSprites[cooldown];
                goTimeOut = true;
            }

            if (cooldown>=0 && cooldown<=0.5f) {
                startTimeOutEvent.Call();
            }
            if (startTimer <= 0) {
                startTimeOut = true;
                startTimerImg.gameObject.SetActive(false);
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
                controllersDetection.CalculateScore();
            }
        }
    }

}
