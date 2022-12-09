using TMPro;
using UnityEngine;

public class ArcadeTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerTextP1; 
    [SerializeField] private TMP_Text _timerTextP2;
    
    private float _timer = 180; //3min
    
    void Update () {
        _timer -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(_timer / 60f);
        int seconds = Mathf.FloorToInt(_timer % 60f);
        int milliseconds = Mathf.FloorToInt((_timer * 100f) % 100f);
        _timerTextP1.text = minutes.ToString ("00") + ":" + seconds.ToString ("00") + ":" + milliseconds.ToString("00");
        _timerTextP2.text = minutes.ToString ("00") + ":" + seconds.ToString ("00") + ":" + milliseconds.ToString("00");

        if (_timer <= 0) {
            TimerOut();
        }
    }

    private void TimerOut() {
        
    }
    
}
