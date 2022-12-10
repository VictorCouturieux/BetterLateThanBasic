using UnityEngine;

public class FadeCam : MonoBehaviour
{
    
    
    [SerializeField] private UnityEngine.UI.Image _darkPanel;
    // [SerializeField] private float _fadeStep = 1;

    // private bool _show 
    private Color _showColor = new Color(0, 0, 0, 1); 
    private Color _hideColor = new Color(0, 0, 0, 0); 

    private void Update() {
        // _darkPanel.CrossFadeColor()
    }

    public void FadeInEvent() {
        
    }
    
    public void FadeOutEvent() {
        
    }
}
