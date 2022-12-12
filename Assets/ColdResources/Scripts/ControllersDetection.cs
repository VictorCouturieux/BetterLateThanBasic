using UnityEngine;
using UnityEngine.InputSystem;

public class ControllersDetection : MonoBehaviour
{
	[SerializeField] private PlayerInput p1;
	[SerializeField] private PlayerInput p2;

	[SerializeField] private GameObject errorGamepadNotConnected;
	[SerializeField] public bool mappedWithGamepads = true;
	
	
	[SerializeField] private VoidGameEvent startTimeOutEvent;
	
	private InputDevice p1Device;
	private InputDevice p2Device;

	private float tempTime;
	private int tempTimer = 0;

	private void OnEnable() {
		startTimeOutEvent.AddCallback(StartTimerOut);
	}
	
	private void StartTimerOut() {
		EnableControlDevice();
	}
	
	private void OnDisable() {
		startTimeOutEvent.RemoveCallback(StartTimerOut);
	}
	
	private void Start() {
		DisableControlDevice();
		
		if (mappedWithGamepads) {
			if (Gamepad.all.Count >= 2) {
	            p1.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
	            p1Device = Gamepad.all[0];
	            p2.SwitchCurrentControlScheme("Gamepad", Gamepad.all[1]);
	            p2Device = Gamepad.all[1];
	        } else if (Gamepad.all.Count == 1) {
	            p1.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
	            p1Device = Gamepad.all[0];
	            p2.SwitchCurrentControlScheme();
	        }
			else {
				p1.SwitchCurrentControlScheme();
				p2.SwitchCurrentControlScheme();
				// p2.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
			}
	        InputSystem.onDeviceChange +=
	            (device, change) => {
		            switch (change) {
		            case InputDeviceChange.Added:
			            if (device is Gamepad) {
				            if (Gamepad.all.Count >= 2) {
					            if (p1Device == null) {
						            p1Device = device;
					            } 
					            if (p2Device == null) {
						            p2Device = device;
					            }
				            } else if (Gamepad.all.Count == 1) {
					            if (p1Device == null) {
						            p1Device = device;
					            } else if (p2Device == null) {
						            p2Device = device;
					            }
				            }
			            }
			            if (p1Device != null) {
				            p1.SwitchCurrentControlScheme("Gamepad", p1Device);
			            }
			            else {
				            p1.SwitchCurrentControlScheme();
			            }
			            if (p2Device != null) {
				            p2.SwitchCurrentControlScheme("Gamepad", p2Device);
			            }
			            else {
				            p2.SwitchCurrentControlScheme();
			            }
			            break;
		            case InputDeviceChange.Removed:
			            // Debug.Log(Gamepad.all.Count);
			            if (Gamepad.all.Count <= 1) {
				            if (device == p1Device) {
					            p1Device = null;
				            }
				            if (device == p2Device) {
					            p2Device = null;
				            }
			            }
			            if (p1Device != null) {
				            p1.SwitchCurrentControlScheme("Gamepad", p1Device);
			            }
			            else {
				            p1.SwitchCurrentControlScheme();
			            }
			            if (p2Device != null) {
				            p2.SwitchCurrentControlScheme("Gamepad", p2Device);
			            }
			            else {
				            p2.SwitchCurrentControlScheme();
			            }
			            break;
		            case InputDeviceChange.Reconnected:
			            // Plugged back in.
			            // Debug.Log("p1 : " + p1.devices.Count + "p2 : " + p2.devices.Count);
			            // Debug.Log("Device Reconnected: " + device.description.interfaceName);
			            break;
		            case InputDeviceChange.Disconnected:
			            // Device got unplugged.
			            // Debug.Log("p1 : " + p1.devices.Count + "p2 : " + p2.devices.Count);
			            // Debug.Log("device Disconnected: " + device.description.interfaceName);
			            break;
		            default:
			            // See InputDeviceChange reference for other event types.
			            break;
		            }
	            };
		}
	}

	private void Update() {
		if (tempTimer < 2) {
			tempTime += Time.deltaTime;
			tempTimer = Mathf.FloorToInt(tempTime % 60f);
		}
		else {
			if (mappedWithGamepads) {
				if (p1Device == null || p2Device == null ) {
					// freeze game + ui error message 
					errorGamepadNotConnected.SetActive(true);
					Time.timeScale = 0;
				}
				else {
					errorGamepadNotConnected.SetActive(false);
					Time.timeScale = 1.0f;
				}
			}
		}
	}
	
	private void EnableControlDevice() {
		if (p1.devices.Count >= 1) {
			InputSystem.EnableDevice(p1.devices[0]);
		}
		if (p2.devices.Count >= 1) {
			InputSystem.EnableDevice(p2.devices[0]);
		}
	}
	
	private void DisableControlDevice() {
		if (p1.devices.Count >= 1) {
			InputSystem.DisableDevice(p1.devices[0]);
		}
		if (p2.devices.Count >= 1) {
			InputSystem.DisableDevice(p2.devices[0]);
		}
	}

}


