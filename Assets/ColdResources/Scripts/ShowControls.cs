using UnityEngine;

public class ShowControls : MonoBehaviour
{
    
	[SerializeField] private VoidGameEvent showControlsEvent;
	[SerializeField] private GameObject showControlsImage;

	private void OnEnable() {
		showControlsEvent.AddCallback(OnShowControls);
	}

	private void OnShowControls() {
		showControlsImage.SetActive(!showControlsImage.activeSelf);
	}

	private void OnDisable() {
		showControlsEvent.RemoveCallback(OnShowControls);
	}
}
