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
	[SerializeField] private VoidGameEvent endFadeInEvent;

	[SerializeField] private VoidGameEvent startGameEvent;
	[SerializeField] private VoidGameEvent exitGameEvent;
	[SerializeField] private VoidGameEvent restartGameEvent;
	[SerializeField] private VoidGameEvent mainMenuEvent;
	[SerializeField] private CalculatedScoreEvent timerOutEvent;

	[HideInInspector] public int scorePlayer1 = 0;
	[HideInInspector] public int scorePlayer2 = 0;
	
	private void Awake() {
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
		if (SceneManager.GetActiveScene().name == gameScene) {
			scorePlayer1 = 0;
			scorePlayer2 = 0;
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
		endFadeInEvent.AddCallback(EndFadeIn);
		startGameEvent.AddCallback(OnStartGame);
		exitGameEvent.AddCallback(OnExitGame);
		restartGameEvent.AddCallback(OnRestartGame);
		mainMenuEvent.AddCallback(OnMainMenuGame);
		timerOutEvent.AddCallback(RaceFinish);
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
		// startFadeInEvent.Call();
		SceneManager.LoadScene(scoreMenuScene);
	}

	private void EndFadeIn() {
		//todo : faire que les fade sois des event avec un string pour savoir sur quel scene ils vont
	}

	private void OnDisable() {
		endFadeInEvent.RemoveCallback(EndFadeIn);
		startGameEvent.RemoveCallback(OnStartGame);
		exitGameEvent.RemoveCallback(OnExitGame);
		restartGameEvent.RemoveCallback(OnRestartGame);
		mainMenuEvent.RemoveCallback(OnMainMenuGame);
		timerOutEvent.RemoveCallback(RaceFinish);
	}

}
