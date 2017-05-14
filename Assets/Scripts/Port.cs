using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Port : MonoBehaviour {

    public List<Transform> outs = new List<Transform>();
    private Random _random = new Random();

    void OnTriggerEnter(Collider other)
    {
        var player = GameObject.Find("Player(Clone)");

		//Esteban --- für LevelAnim
		//var player = GameObject.Find("BTA_Player 1(Clone)");
        if(player != null)
        {
            var random = _random.Next(0,outs.Count);
            var o = outs[random];
            player.transform.position = o.position;
        }
    }

}
