using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour {

	private bool enemyInRange;
	public GameObject LocalPlayer;
	private GameObject enemy;

	void Start(){
		enemyInRange = false;

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && other.gameObject != LocalPlayer) {
			//Debug.Log ("its " + other.gameObject.tag);
			enemyInRange = true;
			enemy = other.gameObject;
		}
	}

	public bool EnemyIsInRange(){
		return enemyInRange;
	}
	public Hitpoints HitEnemy(){
		return enemy.GetComponent<Hitpoints>();
	}



}
