using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Port : MonoBehaviour {

    public List<Transform> outs = new List<Transform>();
    private Random _random = new Random();

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var random = _random.Next(0,outs.Count);
            var o = outs[random];
            other.transform.position = o.position;
        }
    }

}
