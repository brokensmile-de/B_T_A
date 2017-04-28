using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Player : MonoBehaviour {

	public float speed;

	// for animation ----- HIER
	static Animator anim;

	private Rigidbody myrid;

	private Vector3 moveInput;

	private Vector3 moveSpeed;

	private Camera mycam;

	//public TEST_GunController gun;

	public float ShootTime;

	private float aniAcc;

	private bool Fire;

	// Use this for initialization
	void Start () {
		myrid = GetComponent<Rigidbody> ();
		mycam = FindObjectOfType<Camera> ();
		anim = GetComponent<Animator> ();
		aniAcc = ShootTime;
		Fire = false;
	}

	// Update is called once per frame
	void Update () {
		//movement of player
		moveInput = new Vector3 (Input.GetAxisRaw("Horizontal"),0.0f,Input.GetAxisRaw("Vertical"));
		moveSpeed = moveInput * speed;

		// finding direction to look;
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		Ray cameraRay = mycam.ScreenPointToRay (Input.mousePosition);
		float rayLength;

		if (moveSpeed.x != 0f || moveSpeed.z != 0f) {
			anim.SetBool ("isRunning", true);
		} else {
			anim.SetBool ("isRunning", false);
		}

		if(groundPlane.Raycast(cameraRay, out rayLength)){
			Vector3 pointToLook = cameraRay.GetPoint(rayLength);
			transform.LookAt (new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
		}
		if (Input.GetMouseButtonDown(0)) {
			anim.SetBool ("isFiring", true);
			Fire = true;
		}
			if (Fire){
				aniAcc -= Time.deltaTime;
				if (aniAcc <= 0f) {
					anim.SetBool ("isFiring", false);
					Fire = false;
				}

			}

			}
			void OnTriggerEnter(Collider other){
				if (other.gameObject.CompareTag ("Gun Big")) {
					Debug.Log ("GUN BIG");
					// here can we custom the gun preferencies
					//gun.timeBetweenShots = 2f;
					//gun.ChangeGun (2);
					Destroy (other.gameObject);
				}
				if (other.gameObject.CompareTag ("Gun Medium")) {
					Debug.Log ("GUN MEDIUM");
					//gun.timeBetweenShots = 0.5f;
					//gun.ChangeGun (1);
					Destroy (other.gameObject);
				}
				if (other.gameObject.CompareTag ("Gun Small")) {
					Debug.Log ("GUN SMALL");
					// when touched it changes de properties of the gun
					//gun.timeBetweenShots = 0.01f;
					//gun.ChangeGun (0);
					Destroy (other.gameObject);
				}
			}

			void FixedUpdate(){

				myrid.velocity = moveSpeed;
			}
}
