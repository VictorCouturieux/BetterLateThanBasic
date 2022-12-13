using UnityEngine;

public class DontDestroyVisual : MonoBehaviour
{
	void Awake() {
		transform.parent = null;
		DontDestroyOnLoad (gameObject);
	}
}
