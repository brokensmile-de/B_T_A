using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class testNetworkTransform : NetworkBehaviour {

    [SyncVar]
    private Vector3 syncPos;

    [SerializeField]
    Transform MyTransform;

    [SerializeField]
    float lerpRate = 15;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        TransmitPosition();
        LerpPosition();
	}

    void LerpPosition()
    {
        if (!isLocalPlayer)
            MyTransform.position = Vector3.Lerp(MyTransform.position, syncPos, Time.deltaTime * lerpRate);
    }

    [Command]
    void CmdProvicePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if(isLocalPlayer)
        {
            CmdProvicePositionToServer(MyTransform.position);
        }

    }
}
