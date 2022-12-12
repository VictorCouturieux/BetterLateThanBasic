using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager instance = null;
	public static GameManager Instance => instance;
	
	[SerializeField] private String mainMenuScene;
	[SerializeField] private String gameScene;
	[SerializeField] private String scoreMenuScene;
	
	[SerializeField] private VoidGameEvent startGameEvent;
	[SerializeField] private VoidGameEvent exitGameEvent;
	[SerializeField] private CarRaceFinishEvent carRaceFinishEvent;
	[SerializeField] private VoidGameEvent timerOutEvent;
	
	private void Awake() {
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	private void OnEnable() {
		startGameEvent.AddCallback(OnStartGame);
		exitGameEvent.AddCallback(OnExitGame);
		carRaceFinishEvent.AddCallback(CarRaceFinish);
		timerOutEvent.AddCallback(TimeOut);
	}

	private void OnStartGame() {
		SceneManager.LoadScene(gameScene);
	}
	
	private void OnExitGame() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
		Application.Quit();
	}

	private void CarRaceFinish(GameObject car, float swagPoints) {
		Debug.Log(car.name + " a franchie la ligne d'arrivé");
	}
	
	private void TimeOut() {
		SceneManager.LoadScene(scoreMenuScene);
	}

	private void OnDisable() {
		startGameEvent.RemoveCallback(OnStartGame);
		exitGameEvent.RemoveCallback(OnExitGame);
		carRaceFinishEvent.RemoveCallback(CarRaceFinish);
		timerOutEvent.RemoveCallback(TimeOut);
	}

}
