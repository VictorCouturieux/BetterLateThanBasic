using TMPro;
using UnityEngine;

public class ArcadeTimer : MonoBehaviour
{
    private static ArcadeTimer instance = null;
    public static ArcadeTimer Instance => instance;
    
    [SerializeField] private TMP_Text timerTextP1; 
    [SerializeField] private TMP_Text timerTextP2;
    [SerializeField] private TMP_Text startTimerText;
    
    private float startTimer = 4; //3sec
    public float StartTimer => startTimer;
    private bool startTimeOut = false; 
    public bool StartTimeOut => startTimeOut;
    
    private float timer = 180; //3min
    public float Timer => timer;
    private bool timeOut = false;
    public bool TimeOut => timeOut;

    private void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update () {
        if (!startTimeOut) {
            startTimer -= Time.deltaTime;
            int cooldown = Mathf.FloorToInt(startTimer % 60f);
            startTimerText.text = cooldown.ToString("0");
            if (startTimer <= 0) {
                startTimeOut = true;
                startTimerText.gameObject.SetActive(false);
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
                TimerOut();
            }
        }
    }
    
    private void StartTimerOut() {
        
    }

    private void TimerOut() {
        
    }
    
}
