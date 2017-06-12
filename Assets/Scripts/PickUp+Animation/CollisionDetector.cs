using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class CollisionDetector : MonoBehaviour {

	public  GunController gun;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//Esteban---- hier wird gechecked welche Waffe wird gehoben. 
	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Pistol")){
			gun.PickGun(0);
			Destroy (other.gameObject);
		}
		else if(other.gameObject.CompareTag("AssaultRifle")){
			gun.PickGun(1);
			Destroy (other.gameObject);
		}
		else if(other.gameObject.CompareTag("Shotgun")){
			gun.PickGun(2);
			Destroy (other.gameObject);
		}
		else if(other.gameObject.CompareTag("RailGun")){
			gun.PickGun(3);
			Destroy (other.gameObject);
		}
		else if(other.gameObject.CompareTag("InfiniteDash")){
			Debug.Log ("Infinite dash picked up!!");
			Destroy (other.gameObject);
		}
		else if(other.gameObject.CompareTag("DoubleShield")){
			Debug.Log ("Double shield picked up!!");
			Destroy (other.gameObject);
		}

	}
}
