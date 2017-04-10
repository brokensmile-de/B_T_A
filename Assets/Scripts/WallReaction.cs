using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallReaction : MonoBehaviour {



	public Text countText;

	private int count;

	// Use this for initialization
	void Start () {
		count = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other){
		
		if(other.gameObject.CompareTag("Bullet")){ 
			Destroy (other.gameObject);
			count++;
			SetCountText ();
		}
	}
	void SetCountText(){
		countText.text = "SCORE: " + count.ToString ();
	}
}
