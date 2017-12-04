using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//TODO: should use hashcodes here?
		if (!GlobalStateController.Instance.houses.ContainsKey(gameObject.tag)) {
			GlobalStateController.Instance.houses[gameObject.tag] = false;
		}

		SetSignal ();
	}

	public void visitHouse() {
		GlobalStateController.Instance.houses [gameObject.tag] = true;
	}

	private void SetSignal() {
		if (GlobalStateController.Instance.houses [gameObject.tag]) {
			Rotator[] rotators = transform.GetComponentsInChildren<Rotator> (true);
			foreach (Rotator rotator in rotators) {
				if (rotator.CompareTag ("star")) {
					rotator.transform.gameObject.SetActive (false);
				} else if (rotator.CompareTag ("hat")) {
					rotator.transform.gameObject.SetActive (true);
				}
			}
		}
	}
}