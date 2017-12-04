using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

	public string scene;

	void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag ("Player")) {
			SceneManager.LoadScene (scene);
		}
	}
}
