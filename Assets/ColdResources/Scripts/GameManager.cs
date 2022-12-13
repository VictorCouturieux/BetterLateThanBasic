using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager instance = null;
	public static GameManager Instance => instance;
	
	[SerializeField] private String mainMenuScene;
	[SerializeField] private String gameScene;
	[SerializeField] private String scoreMenuScene;

	// [SerializeField] private VoidGameEvent startFadeInEvent;
	// [SerializeField] private VoidGameEvent endFadeInEvent;

	[SerializeField] private VoidGameEvent startGameEvent;
	[SerializeField] private VoidGameEvent exitGameEvent;
	[SerializeField] private VoidGameEvent restartGameEvent;
	[SerializeField] private VoidGameEvent mainMenuEvent;
	[SerializeField] private CalculatedScoreEvent timerOutEvent;
	[SerializeField] private ShowCarPlayersEvent showCarPlayersEvent;
	
	[HideInInspector] public GameObject carPlayer1;
	[HideInInspector] public GameObject carPlayer2;
	
	[HideInInspector] public int scorePlayer1 = 0;
	[HideInInspector] public int scorePlayer2 = 0;
	
	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}
	
	void Start()
    {
        SetRatio(4, 3);
    }
    void SetRatio(float w, float h)
    {
        if ((((float)Screen.width) / ((float)Screen.height)) > w / h)
        {
            Screen.SetResolution((int)(((float)Screen.height) * (w / h)), Screen.height, true);
        }
        else
        {
            Screen.SetResolution(Screen.width, (int)(((float)Screen.width) * (h / w)), true);
        }
    }

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
		if (SceneManager.GetActiveScene().name == gameScene) {
			scorePlayer1 = 0;
			scorePlayer2 = 0;
			// carPlayer1 = null;
			// carPlayer2 = null;
		}

		WhenSceneLoad();
	}

	private void WhenSceneLoad() {
		if (SceneManager.GetActiveScene().name != gameScene) {
			if (Gamepad.all.Count >= 2) {
				if (!Gamepad.all[0].enabled) {
					InputSystem.EnableDevice(Gamepad.all[0]);
				}
				if (Gamepad.all[1].enabled) {
					InputSystem.DisableDevice(Gamepad.all[1]);
				}
			} else if (Gamepad.all.Count == 1) {
				if (!Gamepad.all[0].enabled) {
					InputSystem.EnableDevice(Gamepad.all[0]);
				}
			} else if (Gamepad.all.Count == 0) {
				if (!Keyboard.current.enabled) {
					InputSystem.EnableDevice(Keyboard.current);
				}
			}
		}
	}

	private void OnEnable() {
		// endFadeInEvent.AddCallback(EndFadeIn);
		startGameEvent.AddCallback(OnStartGame);
		exitGameEvent.AddCallback(OnExitGame);
		restartGameEvent.AddCallback(OnRestartGame);
		mainMenuEvent.AddCallback(OnMainMenuGame);
		timerOutEvent.AddCallback(RaceFinish);
		showCarPlayersEvent.AddCallback(RaceCarFinish);
	}

	private void OnStartGame() {
		// startFadeInEvent.Call();
		SceneManager.LoadScene(gameScene);
	}
	
	private void OnExitGame() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
		Application.Quit();
	}

	private void OnRestartGame() {
		// startFadeInEvent.Call();
		SceneManager.LoadScene(gameScene);
	}
	
	private void OnMainMenuGame() {
		// startFadeInEvent.Call();
		SceneManager.LoadScene(mainMenuScene);
	}

	private void RaceFinish(int scoreP1, int scoreP2) {
		scorePlayer1 = scoreP1;
		scorePlayer2 = scoreP2;
	}
	
	private void RaceCarFinish(GameObject p1, GameObject p2) {
		carPlayer1 = p1;
		carPlayer1.transform.parent = null;
		DontDestroyOnLoad (carPlayer1.gameObject);
		
		carPlayer2 = p2;
		carPlayer2.transform.parent = null;
		DontDestroyOnLoad (carPlayer2.gameObject);
		
		Debug.Log("CarPlayers : " + carPlayer1 + " " + carPlayer2);
		SceneManager.LoadScene(scoreMenuScene);
	}

	private void EndFadeIn() {
		//todo : faire que les fade sois des event avec un string pour savoir sur quel scene ils vont
	}

	private void OnDisable() {
		// endFadeInEvent.RemoveCallback(EndFadeIn);
		startGameEvent.RemoveCallback(OnStartGame);
		exitGameEvent.RemoveCallback(OnExitGame);
		restartGameEvent.RemoveCallback(OnRestartGame);
		mainMenuEvent.RemoveCallback(OnMainMenuGame);
		timerOutEvent.RemoveCallback(RaceFinish);
		showCarPlayersEvent.AddCallback(RaceCarFinish);
	}

}
