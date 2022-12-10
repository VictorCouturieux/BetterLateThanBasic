using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllersDetection : MonoBehaviour
{
	[SerializeField] private PlayerInput p1;
	[SerializeField] private PlayerInput p2;

	
	
	
	private void Start() {
		if (Gamepad.all.Count >= 2) {
			p1.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
			p2.SwitchCurrentControlScheme("Gamepad", Gamepad.all[1]);
		} else if (Gamepad.all.Count == 1) {
			p1.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
		}
		
		
		InputSystem.onDeviceChange +=
			(device, change) => {
				switch (change) {
				case InputDeviceChange.Added:
					// New Device.
					// Debug.Log("New device added: " + device.description.interfaceName);
					if (device is Gamepad) {
						if (p1.devices.Count != 1) {
							p1.SwitchCurrentControlScheme("Gamepad", device);
						} else if (p2.devices.Count != 1) {
							p2.SwitchCurrentControlScheme("Gamepad", device);
						}
					}
					break;
				case InputDeviceChange.Removed:
					// Remove from Input System entirely; by default, Devices stay in the system once discovered.
					// Debug.Log("Device removed: " + device.description.interfaceName);
					if (p1.devices.Count != 1) {
						p1.SwitchCurrentControlScheme("KeyboardMouse", Keyboard.current, Mouse.current);
						Debug.Log("p1 KeyboardMouse");
					} else if (p1.devices.Count != 1) {
						p2.SwitchCurrentControlScheme("KeyboardMouse", Keyboard.current, Mouse.current);
						Debug.Log("p2 KeyboardMouse");
					}
					break;
				case InputDeviceChange.Reconnected:
					// Plugged back in.
					// Debug.Log("Device Reconnected: " + device.description.interfaceName);
					break;
				case InputDeviceChange.Disconnected:
					// Device got unplugged.
					// Debug.Log("device Disconnected: " + device.description.interfaceName);
					break;
				default:
					// See InputDeviceChange reference for other event types.
					break;
				}
			};
	}

	private void Update() {
		if (Gamepad.all.Count != 2) {
			// freeze game + ui error message 
		}
	}
	
}


