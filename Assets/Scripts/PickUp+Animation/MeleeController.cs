using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeController : MonoBehaviour {

	public int Damage;

	public float punchTime;

	public PlayerMovement player;

	public GameObject playerOb;

	public GameObject sphere;

	public SphereController SController;

	private float timeAcc;

	private bool isPunching;

	static Hitpoints enemy;



	// Use this for initialization
	void Start () {
		sphere.SetActive (false);
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
				sphere.SetActive (false);
				//Debug.Log ("Punch out");
			}
		}
		if(!isPunching && Input.GetKeyDown(KeyCode.E)){
			sphere.SetActive (true);
			isPunching = true;
			player.Punch ();
			//Debug.Log ("Punch in");
			if(SController.EnemyIsInRange()){
				Debug.Log ("Punch with Enemy in range");
				//hier kommt der Damage handling
				enemy = SController.HitEnemy();
				enemy.TakeDamage (Damage, playerOb);
			}
		}
	}


}
