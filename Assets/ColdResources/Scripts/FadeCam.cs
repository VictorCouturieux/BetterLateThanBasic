using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeCam : MonoBehaviour
{
    [SerializeField] private Image darkPanel;
    
    private bool isFadeIn = false;
    private bool isFadeOut = false;

    private float fadeTimer = 0.0f;
    
    [SerializeField] private VoidGameEvent startFadeInEvent;
    [SerializeField] private VoidGameEvent startFadeOutEvent;

    [SerializeField] private VoidGameEvent endFadeInEvent;
    [SerializeField] private VoidGameEvent endFadeOutEvent;

    private void Start() {
        // darkPanel.color = new Color(0, 0, 0, 1);
        // startFadeOutEvent.Call();
    }

    private void Update() {
        if (isFadeIn) {
            fadeTimer += Time.deltaTime;
            darkPanel.color = new Color(0, 0, 0, fadeTimer);
            if (fadeTimer >= 1) {
                fadeTimer = 1;
                isFadeIn = false;
                endFadeInEvent.Call();
            }
        }
        else if(isFadeOut) {
            fadeTimer -= Time.deltaTime;
            darkPanel.color = new Color(0, 0, 0, fadeTimer);
            if (fadeTimer <= 0) {
                fadeTimer = 0;
                isFadeOut = false;
                endFadeOutEvent.Call();
            }
        }
    }

    private void OnEnable() {
        startFadeInEvent.AddCallback(FadeIn);
        startFadeOutEvent.AddCallback(FadeOut);
        
    }

    public void FadeIn() {
        fadeTimer = 0;
        isFadeIn = true;
        isFadeOut = false;
    }
    
    public void FadeOut() {
        fadeTimer = 1;
        isFadeOut = true;
        isFadeIn = false;
    }
    
    private void OnDisable() {
        startFadeInEvent.RemoveCallback(FadeIn);
        startFadeOutEvent.RemoveCallback(FadeOut);
    }


}
