using UnityEngine;

public class RotateBodyCarPart : MonoBehaviour
{
    [Header("Choose on witch axe the looter rotate")]
    [SerializeField] private bool X;
    [SerializeField] private bool Y;
    [SerializeField] private bool Z;
    [SerializeField] private bool upAndDown;
    
    private float value;
    private void Update()
    {
        if (X) transform.Rotate(new Vector3(Time.deltaTime * 100,0,0));
        if (Y) transform.Rotate(new Vector3(0,Time.deltaTime * 100,0));
        if (Z) transform.Rotate(new Vector3(0,0,Time.deltaTime * 100));
        if (upAndDown)transform.localPosition = new Vector3(0,.5f +Mathf.Sin(value += Time.deltaTime )/2,0);
    }
}
