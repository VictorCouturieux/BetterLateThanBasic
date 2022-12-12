using UnityEngine;
using UnityEngine.InputSystem;

public class ControllersDetection : MonoBehaviour
{
	[SerializeField] private GameObject player1;
	[SerializeField] private GameObject player2;
	
	[SerializeField] private int bonusWhenFirst = 6;
	[SerializeField] private int bonusWhenSecond = 3;
	
	private PlayerInput p1PlayerInput;
	private PlayerInput p2PlayerInput;

	private BodyCarPartsManager p1BodyCarParts;
	private int bonusSwagPointsP1 = 0;
	private BodyCarPartsManager p2BodyCarParts;
	private int bonusSwagPointsP2 = 0;

	[SerializeField] private GameObject errorGamepadNotConnected;
	[SerializeField] public bool mappedWithGamepads = true;

	[SerializeField] private ArcadeTimer arcadeTimer;

	[SerializeField] private VoidGameEvent startTimeOutEvent;
	[SerializeField] private CarRaceFinishEvent carRaceFinishEvent;
	[SerializeField] private CalculatedScoreEvent timerOutEvent;
	
	private InputDevice p1Device;
	private InputDevice p2Device;

	private float tempTime;
	private int tempTimer = 0;

	private void Awake() {
		p1PlayerInput = player1.GetComponent<PlayerInput>();
		p2PlayerInput = player2.GetComponent<PlayerInput>();

		p1BodyCarParts = player1.GetComponent<BodyCarPartsManager>();
		p2BodyCarParts = player2.GetComponent<BodyCarPartsManager>();
	}

	private void OnEnable() {
		startTimeOutEvent.AddCallback(StartTimerOut);
		carRaceFinishEvent.AddCallback(CarRaceFinish);
	}
	
	private void StartTimerOut() {
		EnableControlDevice(p1PlayerInput);
		EnableControlDevice(p2PlayerInput);
	}
	
	private void CarRaceFinish(GameObject car, float swagPoints) {
		Debug.Log(car.name + " a franchie la ligne d'arrivé");
		if (car.GetComponent<PlayerInput>() == p1PlayerInput) {
			DisableControlDevice(p1PlayerInput);
			if (bonusSwagPointsP2 == 0) {
				bonusSwagPointsP1 = bonusWhenFirst;
			}else {
				bonusSwagPointsP1 = bonusWhenSecond;
			}
		}else if (car.GetComponent<PlayerInput>() == p2PlayerInput) {
			DisableControlDevice(p2PlayerInput);
			if (bonusSwagPointsP1 == 0) {
				bonusSwagPointsP2 = bonusWhenFirst;
			}else {
				bonusSwagPointsP2 = bonusWhenSecond;
			}
		}
		if (!p1PlayerInput.devices[0].enabled && !p2PlayerInput.devices[0].enabled) {
			CalculateScore();
		}
	}

	public void CalculateScore() {
		int totalPointP1 = p1BodyCarParts.playerScore + bonusSwagPointsP1;
		int totalPointP2 = p2BodyCarParts.playerScore + bonusSwagPointsP2;
		timerOutEvent.Call(totalPointP1, totalPointP2);
	}
	
	private void OnDisable() {
		startTimeOutEvent.RemoveCallback(StartTimerOut);
		carRaceFinishEvent.RemoveCallback(CarRaceFinish);
	}
	
	private void Start() {
		DisableControlDevice(p1PlayerInput);
		DisableControlDevice(p2PlayerInput);
		
		if (mappedWithGamepads) {
			if (Gamepad.all.Count >= 2) {
	            p1PlayerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
	            p1Device = Gamepad.all[0];
	            p2PlayerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[1]);
	            p2Device = Gamepad.all[1];
	        } else if (Gamepad.all.Count == 1) {
	            p1PlayerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
	            p1Device = Gamepad.all[0];
	            p2PlayerInput.SwitchCurrentControlScheme();
	        }
			else {
				p1PlayerInput.SwitchCurrentControlScheme();
				p2PlayerInput.SwitchCurrentControlScheme();
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
				            p1PlayerInput.SwitchCurrentControlScheme("Gamepad", p1Device);
			            }
			            else {
				            p1PlayerInput.SwitchCurrentControlScheme();
			            }
			            if (p2Device != null) {
				            p2PlayerInput.SwitchCurrentControlScheme("Gamepad", p2Device);
			            }
			            else {
				            p2PlayerInput.SwitchCurrentControlScheme();
			            }
			            if (!arcadeTimer.StartTimeOut) {
				            DisableControlDevice(p1PlayerInput);
				            DisableControlDevice(p2PlayerInput);
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
				            p1PlayerInput.SwitchCurrentControlScheme("Gamepad", p1Device);
			            }
			            else {
				            p1PlayerInput.SwitchCurrentControlScheme();
			            }
			            if (p2Device != null) {
				            p2PlayerInput.SwitchCurrentControlScheme("Gamepad", p2Device);
			            }
			            else {
				            p2PlayerInput.SwitchCurrentControlScheme();
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
	
	private void EnableControlDevice(PlayerInput playerInput) {
		if (playerInput.devices.Count >= 1) {
			InputSystem.EnableDevice(playerInput.devices[0]);
		}
	}
	
	private void DisableControlDevice(PlayerInput playerInput) {
		if (playerInput.devices.Count >= 1) {
			InputSystem.DisableDevice(playerInput.devices[0]);
		}
	}

}


