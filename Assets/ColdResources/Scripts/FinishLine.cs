using UnityEngine;
using UnityEngine.InputSystem;

public class FinishLine : MonoBehaviour
{
	[SerializeField] private CarRaceFinishEvent carRaceFinishEvent;
	
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			// carRaceFinishEvent.Call(other.gameObject.GetComponent<PlayerInput>(), 0f);
			carRaceFinishEvent.Call(other.gameObject, 0f);
		}
	}
}
