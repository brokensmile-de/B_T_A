using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Weist dem Lokalen Player Character während der runtime die Kamera zu
public class AttachLocalCamera : NetworkBehaviour {
    private GameObject cameraObject;
    private CameraFollow cameraScript;
    private GameObject player;
	
	void Start () {

        //damit tritt das Script nur beim local player in Kraft( Ansonsten würde es die anderen Player auch beeinflussen)
        if (!isLocalPlayer)
        {
            return;
        }

        cameraObject =GameObject.FindGameObjectWithTag("MainCamera");
        player = this.gameObject;        
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();        
        cameraScript.setTarget(player);        
    }
	

}
