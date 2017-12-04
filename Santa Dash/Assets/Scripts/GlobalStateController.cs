using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalStateController : MonoBehaviour {

	public static GlobalStateController Instance;

	private float timeLimit = 60.0f;
	private bool won = false;
	private bool lost = false;

	public Vector3 lastOutdoorPos;
	public Vector3 playerScale;
	public float playerMass;
	public Dictionary<string, bool> houses = new Dictionary<string, bool> ();
	public GUIStyle guistyle;

	void Awake () {
		if (Instance == null) {
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	void Update() {
		if (!(won || lost)) UpdateGameClock ();
		CheckGameState ();
	}

	void UpdateGameClock() {
		timeLimit -= Time.deltaTime;
	}

	void OnGUI() {
		if (!(won || lost)) {
			GUI.Label (new Rect (10, 10, 1000, 30), "Timer: " + timeLimit.ToString (), guistyle);
		}
	}

	void CheckGameState() {
		// check if time has run out or player is too tiny
		// if not, check if player has hit all houses
		if ((timeLimit <= 0 || IsTooTiny()) && !lost) {
			lost = true;
			SceneManager.LoadScene ("lose");
		}
		if ((!IsMissingHouses ()) && !won) {
			won = true;
			SceneManager.LoadScene ("win");
		}

		Scene currentScene = SceneManager.GetActiveScene ();
		if (currentScene.name == "win" || currentScene.name == "lose") {
			if (Input.anyKey) {
				Reset ();
			}
		}
	}

	private bool IsMissingHouses() {
		int visited = 0;
		foreach(string house in houses.Keys) {
			if (houses[house]) {
				++visited;
			}
		}

		return visited != houses.Keys.Count || houses.Keys.Count == 0;
	}

	private void Reset() {
		won = false;
		lost = false;
		timeLimit = 60.0f;

		lastOutdoorPos = Vector3.zero;
		playerScale = Vector3.one;
		playerMass = 2.0f;
		houses.Clear ();

		SceneManager.LoadScene ("outdoors");
	}

	private bool IsTooTiny() {
		return playerScale.y <= 0.11f || playerMass <= 1.1f;
	}
}
