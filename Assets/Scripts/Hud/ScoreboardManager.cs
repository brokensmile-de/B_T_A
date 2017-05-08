using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour {

    public GameObject scoreboard;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKey("tab"))
            scoreboard.SetActive(true);
        else
        {
            if(scoreboard.activeInHierarchy)
                scoreboard.SetActive(false);
        }
    }
}

