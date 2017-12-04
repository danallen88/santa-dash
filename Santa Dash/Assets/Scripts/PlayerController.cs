using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	public static float RADIAL_SCALE = 0.001f;
	public static float JOLLY_STEP = 0.001f;
	public float speed;

	private Rigidbody rb;
	private Vector3 movement = Vector3.zero;

	private bool jump = false;
	private bool isGrounded;
	private float jumpSpeed = 2.0f;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		LoadState ();
	}

	void Update () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		movement.x = moveHorizontal;
		movement.z = moveVertical;

		jump = Input.GetButton ("Jump");
	}
	
	void FixedUpdate () {
		Move ();
		Jump ();
	}
		
	void Move() {
		rb.AddForce (movement * speed);

		if (Input.GetButton ("Horizontal") || Input.GetButton ("Vertical")) {
			AdjustPlayerSize (new Vector3(-RADIAL_SCALE, -RADIAL_SCALE, -RADIAL_SCALE));
			rb.mass -= JOLLY_STEP;
			GlobalStateController.Instance.playerMass -= JOLLY_STEP;
			if (rb.mass <= 1.0f) {
				rb.mass = 1.0f;
				GlobalStateController.Instance.playerMass = 1.0f;
			}

			if (transform.localScale.y <= 0.1f) {
				transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				GlobalStateController.Instance.playerScale = new Vector3(0.1f, 0.1f, 0.1f);
			}
		}
	}

	void Jump() {
		isGrounded = Physics.Raycast (transform.position, Vector3.down, 1.0f);

		if (jump && isGrounded) {
			rb.AddForce (Vector3.up * jumpSpeed, ForceMode.Impulse);
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag ("chimney")) {
			HouseController house = collider.GetComponentInParent<HouseController> ();
			house.visitHouse ();
			SaveState ();		
		}
	}

	void OnTriggerStay(Collider collider) {
		if (collider.CompareTag ("cookies")) {
			AdjustPlayerSize (new Vector3(2*RADIAL_SCALE, 2*RADIAL_SCALE, 2*RADIAL_SCALE));
			rb.mass += 2*JOLLY_STEP;
			GlobalStateController.Instance.playerMass += (2 * JOLLY_STEP);
		}
	}

	void SaveState () {
		GlobalStateController.Instance.lastOutdoorPos = transform.position;
		GlobalStateController.Instance.playerScale = transform.localScale;
		GlobalStateController.Instance.playerMass = rb.mass;
	}

	void LoadState() {
		transform.localScale = GlobalStateController.Instance.playerScale;
		rb.mass = GlobalStateController.Instance.playerMass;
		Scene currentScene = SceneManager.GetActiveScene ();

		if (currentScene.name == "outdoors") {
			Vector3 resetPos = GlobalStateController.Instance.lastOutdoorPos + new Vector3(-5.0f, 0.0f, -5.0f);	// Nudge player away from house
			resetPos.y = 0.5f;	// Want to start player on ground
			transform.position = resetPos;
		}
	}

	void AdjustPlayerSize(Vector3 scale) {
		transform.localScale += scale;
		GlobalStateController.Instance.playerScale += scale;
	}
}
