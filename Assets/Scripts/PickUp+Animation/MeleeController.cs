﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour {

	public float punchTime;

	public PlayerMovement player;

	private float timeAcc;

	private bool isPunching;



	// Use this for initialization
	void Start () {
		timeAcc = punchTime;
		isPunching = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isPunching) {
			timeAcc -= Time.deltaTime;
			if (timeAcc <= 0f) {
				isPunching = false;
				timeAcc = punchTime;
				player.StopPunch();
				//Debug.Log ("Punch out");
			}
		}
		if(!isPunching && Input.GetKeyDown(KeyCode.E)){
			isPunching = true;
			player.Punch ();
			//Debug.Log ("Punch in");
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Wall") && isPunching){
			Debug.Log ("Wall Punch");
		}
	}
}