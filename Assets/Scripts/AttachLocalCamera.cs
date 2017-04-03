using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttachLocalCamera : NetworkBehaviour
{
    private GameObject cameraObject;
    private CameraLock cameraScript;
    private GameObject player;

    void Start()
    {

        //damit tritt das Script nur beim local player in Kraft( Ansonsten würde es die anderen Player auch beeinflussen)
        if (!isLocalPlayer)
        {
            return;
        }

        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        player = this.gameObject;
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLock>();
        cameraScript.setTarget(player);
    }


}

