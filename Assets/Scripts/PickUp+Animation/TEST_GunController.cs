using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_GunController : MonoBehaviour {

	public GameObject gun1;

	private GameObject currentGun;

	public Transform gunHolder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PickGun(int i){
		if (i == 0) {
			currentGun = Instantiate (gun1, gunHolder.position, gunHolder.rotation);
			currentGun.transform.parent = gunHolder;
		}
	}
}
