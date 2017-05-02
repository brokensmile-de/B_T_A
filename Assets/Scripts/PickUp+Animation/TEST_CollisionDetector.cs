using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_CollisionDetector : MonoBehaviour {

	public TEST_GunController gun;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Gun")){
			Debug.Log ("Gun picked up");
			gun.PickGun(0);
			Destroy (other.gameObject);
		}
	}
}
